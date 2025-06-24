
// ContinueDiscoveringDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"

#include "Discoverer.h"


// CContinueDiscoveringDlg dialog
class CContinueDiscoveringDlg : public CDialogEx
{
// Construction
public:
	CContinueDiscoveringDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_CONTINUEDISCOVERING_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

private:
	CListCtrl lvDevices;
	CListBox lbLog;

	CDiscoverer* FDisc;

	void DiscOnDeviceLost(void* sender, const __int64 Address);
	void DiscOnDeviceFound(void* sender, const __int64 Address);
	void DiscOnStopped(void* sender);
	void DiscOnStarted(void* sender);

public:
	afx_msg void OnBnClickedButtonClear();
	afx_msg void OnDestroy();
	afx_msg void OnBnClickedButtonStart();
	afx_msg void OnBnClickedButtonStop();
};
