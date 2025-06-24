
// ContinueDiscovering.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CContinueDiscoveringApp:
// See ContinueDiscovering.cpp for the implementation of this class
//

class CContinueDiscoveringApp : public CWinApp
{
public:
	CContinueDiscoveringApp();

// Overrides
public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CContinueDiscoveringApp theApp;