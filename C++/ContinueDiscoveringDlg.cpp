
// ContinueDiscoveringDlg.cpp : implementation file
//

#include "stdafx.h"
#include "ContinueDiscovering.h"
#include "ContinueDiscoveringDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CString IntToHex(const int i)
{
	CString s;
	s.Format(_T("%.8X"), i);
	return s;
}

CString IntToHex(const __int64 i)
{
	CString s;
	s.Format(_T("%.4X%.8X"), static_cast<INT32>((i >> 32) & 0x00000FFFF),
		static_cast<INT32>(i) & 0xFFFFFFFF);
	return s;
}


// CContinueDiscoveringDlg dialog



CContinueDiscoveringDlg::CContinueDiscoveringDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_CONTINUEDISCOVERING_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CContinueDiscoveringDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_DEVICES, lvDevices);
	DDX_Control(pDX, IDC_LIST_LOG, lbLog);
}

BEGIN_MESSAGE_MAP(CContinueDiscoveringDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_CLEAR, &CContinueDiscoveringDlg::OnBnClickedButtonClear)
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_START, &CContinueDiscoveringDlg::OnBnClickedButtonStart)
	ON_BN_CLICKED(IDC_BUTTON_STOP, &CContinueDiscoveringDlg::OnBnClickedButtonStop)
END_MESSAGE_MAP()


// CContinueDiscoveringDlg message handlers

BOOL CContinueDiscoveringDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	lvDevices.InsertColumn(0, _T("Address"), 0, 150);
	lvDevices.InsertColumn(1, _T("Name"), 0, 150);
	lvDevices.SetExtendedStyle(lvDevices.GetExtendedStyle() | LVS_EX_FULLROWSELECT);

	FDisc = new CDiscoverer();
	__hook(&CDiscoverer::OnStarted, FDisc, &CContinueDiscoveringDlg::DiscOnStarted);
	__hook(&CDiscoverer::OnStopped, FDisc, &CContinueDiscoveringDlg::DiscOnStopped);
	__hook(&CDiscoverer::OnDeviceFound, FDisc, &CContinueDiscoveringDlg::DiscOnDeviceFound);
	__hook(&CDiscoverer::OnDeviceLost, FDisc, &CContinueDiscoveringDlg::DiscOnDeviceLost);

	int Res = FDisc->Open();
	if (Res != WCL_E_SUCCESS)
		lbLog.AddString(_T("Open failed; 0x") + IntToHex(Res));

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CContinueDiscoveringDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CContinueDiscoveringDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CContinueDiscoveringDlg::OnBnClickedButtonClear()
{
	lbLog.ResetContent();
}

void CContinueDiscoveringDlg::DiscOnDeviceLost(void* sender, __int64 const Address)
{
	lbLog.AddString(_T("Device lost: ") + IntToHex(Address));
	if (lvDevices.GetItemCount() > 0)
	{
		for (int i = 0; i < lvDevices.GetItemCount(); i++)
		{
			CString s = lvDevices.GetItemText(i, 0);
			if (s == IntToHex(Address))
			{
				lvDevices.DeleteItem(i);
				break;
			}
		}
	}
}

void CContinueDiscoveringDlg::DiscOnDeviceFound(void* sender, const __int64 Address)
{
	lbLog.AddString(_T("Device found: ") + IntToHex(Address));
	int Item = lvDevices.GetItemCount();
	lvDevices.InsertItem(Item, IntToHex(Address));

	tstring Name;
	int Res = FDisc->Radio->GetRemoteName(Address, Name);
	if (Res != WCL_E_SUCCESS)
		Name = _T("Error: 0x") + IntToHex(Res);

	lvDevices.SetItemText(Item, 1, Name.c_str());
}

void CContinueDiscoveringDlg::DiscOnStopped(void* sender)
{
	lbLog.AddString(_T("Stopped"));
	
	lvDevices.DeleteAllItems();
}

void CContinueDiscoveringDlg::DiscOnStarted(void* sender)
{
	lbLog.AddString(_T("Started"));
}

void CContinueDiscoveringDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	FDisc->Stop();
	FDisc->Close();
	__unhook(FDisc);
	delete FDisc;
}

void CContinueDiscoveringDlg::OnBnClickedButtonStart()
{
	int Res = FDisc->Start();
	if (Res != WCL_E_SUCCESS)
	{
		lbLog.AddString(_T("Start failed: 0x") + IntToHex(Res));
		FDisc->Close();
	}
}

void CContinueDiscoveringDlg::OnBnClickedButtonStop()
{
	int Res = FDisc->Stop();
	if (Res != WCL_E_SUCCESS)
		lbLog.AddString(_T("Stop failed: 0x") + IntToHex(Res));
}
