//---------------------------------------------------------------------------

#pragma hdrstop

#include "CAMOAR.h"

//---------------------------------------------------------------------------

#pragma package(smart_init)

//// TESTS
HRESULT hr_st = S_OK;

//-----------------------------------------------------------------------------
// Name: GetContrastValueOAR()
// Desc:
//-----------------------------------------------------------------------------
__inline UINT16 GetContrastValueOAR( UINT16 uiValue, UINT16 uwMin,  UINT16 uwMax)
{
    if (uiValue < uwMin)
        {
            return 0;
        }

    if (uiValue > uwMax)
        {
            return 0xFF;
        }


    float fTMP = ( (float)( uiValue - uwMin )) / ((float)(uwMax - uwMin) ) * 256.0f;
    return (UINT16)fTMP;
}

//-----------------------------------------------------------------------------
//
//  class TCamCAMOAR
//
//-----------------------------------------------------------------------------
CD3DBitmap * __fastcall TCamCAMOAR::GetD3DBitmap(UINT32 dwIndex)
{
	if (dwIndex == 0)
    	return m_pD3DBitmap;
	else
       	if (dwIndex == 1)
	    	return m_pD3DbitmapGistogramm;
		else
        	return NULL;
};

HBITMAP __fastcall TCamCAMOAR::GetHBitmap(UINT32 dwIndex)
{
	if (dwIndex == 0)
    	return m_hbmBitmap;
	else
       	if (dwIndex == 1)
	    	return m_hbmGistogramm;
		else
        	return NULL;
};

//-----------------------------------------------------------------------------
// Name: InitializeData()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall  TCamCAMOAR::InitializeData()
{
    m_Settings.m_bDirectDraw    = false;        // Режим прямого видео отключен

    m_avi               = NULL;     // Video Capture
    m_bCapturing        = false;    // Запись не ведется
    m_bError            = false;    // Наличие ошибок кодека при записи
    m_bFirstTime        = true;     // Необходимо отобразить диалог выбора кодека
    m_Settings.m_iSleepCounter     = 0;        // Frame counter between capture
    m_Settings.m_iFrameCounter     = 0;        // Video Frame counter
    m_Settings.m_bCaptureOn        = false;
    m_Settings.m_bNeedCompress     = true;     // Необходимо сжимать видеоданные

	m_uiBmpCnt			= 2;

    m_bHasData			= false;

	m_hbmBitmap			= NULL;
    m_pD3DBitmap		= NULL;

    // Gistogramm
	m_hbmGistogramm		= NULL;
    m_pD3DbitmapGistogramm = NULL;

	m_Settings.m_dwDataSizeX		= 320;
	m_Settings.m_dwDataSizeY		= 256;
	m_Settings.m_dwDataSize			= m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY * sizeof(UINT16);

	m_Settings.m_dwGistSizeX		= 200;
	m_Settings.m_dwGistSizeY		= 200;

	m_Settings.m_bGistogramm		= false;
	m_Settings.m_bAverage           = false;
	m_Settings.m_bMin               = false;
	m_Settings.m_bMax               = false;
	m_Settings.m_dwShift    		= 7;

	m_Settings.m_dwMax				= 0;
	m_Settings.m_dwMaxIndex 		= 0;

  	m_Settings.m_wMinLev            = 0;
  	m_Settings.m_wMaxLev            = 0xFFFF;

	m_Settings.m_bDarkFrame         = false;    // Dark Frame Mode
	m_Settings.m_bDarkFrameInited   = false;    // Dark Frame Mode Inited
	m_Settings.m_bSubDarkFrame      = false;    // Subtract Dark Frame
	m_Settings.m_dwFrameAdded       = 0;        // Frame Counter

	m_Settings.m_swDumpCount        = 4;

  	m_Settings.m_dwContrast         = 0;    // Контрастер отключен

    // Broken Pixels Correction
    m_Settings.m_bBrokenPixels      = false;
    m_Settings.m_bFindBrokenPixels  = false;
    m_Settings.m_dwBadPixelsCount   = 0;

    //------------------------------------------------------------------
    // Настройка данных для отправки по умолчанию
    //------------------------------------------------------------------
    // Запрос статуса
  	m_Settings.OARStatusReq.fFreq = 10.0;
  	m_Settings.OARStatusReq.uiByte1  = 3;   // Регулярная с заданной частотой ; Разрешить выдачу статуса без запроса
    // Режим модуля
  	m_Settings.OARModuleMode.uiByte1 = 0;
  	m_Settings.OARModuleMode.uiWord1 = 300;
   	m_Settings.OARModuleMode.uiByte2 = 0;
  	m_Settings.OARModuleMode.uiByte3 = 0;    
  	m_Settings.OARModuleMode.uiByte4 = 0;
  	m_Settings.OARModuleMode.uiByte5 = 0;
  	m_Settings.OARModuleMode.uiByte6 = 0;
  	m_Settings.OARModuleMode.uiWord2 = 3000;
  	m_Settings.OARModuleMode.uiByte7 = 3;
  	m_Settings.OARModuleMode.uiByte8 = 0;
  	m_Settings.OARModuleMode.uiDWord1 = 0;
    // Запись параметров
   	m_Settings.OARParametersSendHead.uiByte1 = 3;   // EEPROM
   	m_Settings.OARParametersSendHead.uiByte2 = 0;
   	m_Settings.OARParametersSendHead.uiByte3 = 0;
   	m_Settings.OARParametersSendHead.uiByte4 = 0;
    // Запрос параметров
    m_Settings.OARParamReq.uiByte1 = 3; // EEPROM
    m_Settings.OARParamReq.uiByteReserve1 = 0;
    m_Settings.OARParamReq.uiByteReserve2 = 0;
    m_Settings.OARParamReq.uiByteReserve3 = 0;
    m_Settings.OARParamReq.uiDWord1 = 0xA500;
    m_Settings.OARParamReq.uiDWord2 = 100;

    // Установка количества данных для отправки в EEPROM
    m_Settings.uiEEPROMDataCount = 520;

    // Установка количества данных для отправки в RAM
    m_Settings.uiRAMDataCount    = 130;

    // Установка начального адреса для записи в EEPROM
    m_Settings.uiEEPROMInitAddr = 0xA500;

    // Установка начального адреса для записи в RAM
    m_Settings.uiRAMInitAddr    = 0x40000;
    m_Settings.uiRAMInitBank    = 0;

    for (int i = 0; i < 521; i++)
        {
            m_Settings.OAREEPROMData[i].uwAddr = m_Settings.uiEEPROMInitAddr + i;
            m_Settings.OAREEPROMData[i].ucData = i;
            m_Settings.OAREEPROMData[i].ucReserve = 0;
        }

    for (int i = 0; i < 130; i++)
        {
            m_Settings.OARRAMData[i].uiAddr = (1 << 19) | ( (m_Settings.uiRAMInitAddr + i * 4) & 0x7FFFF );
            m_Settings.OARRAMData[i].uiData = i;
        }

    // Количество страниц
    m_Settings.uiFLASHPageCount = 4;
    // Заполнение начальных данных FLASH
    m_Settings.OARFlashData[0].uiAddr = 0x00000000;
    m_Settings.OARFlashData[1].uiAddr = 0x00000200;
    m_Settings.OARFlashData[2].uiAddr = 0x00000400;
    m_Settings.OARFlashData[3].uiAddr = 0x00000800;


    // Заполнение данных по новому протоколу
    m_Settings.uiDataCountNew   = 2000;         // Количество данных, которое будет отправлено по новому протоколу
    m_Settings.uiInitAddrNew    = 0x00000000;   // Начальный адрес по новому протоколу
    for (int i = 0; i < 2000; i++ )
        m_Settings.OARDataToSendNew[i] = 0xFFFFFFFF;               // Данные для отправки по новому протоколу


    // Нет полученных данных
    m_Settings.bDataArrived = false;

    // Установка режима начального отображения служебной информации(Гистограмма, навигационные данные)
    m_Settings.m_Reg    = 0;    // Отображать гистограмму
    m_Settings.m_Nav    = 0;    // Отображать матрицу МГД
    m_Settings.m_CurStat = 0;   // Отображать общий статус

    // Идентификатор камеры по умолчанию
    m_Settings.Device_ID    = 0x41;
    m_Settings.D_ID         = 0x4FFF;

    VOGMatrix3x3Identity( &m_Settings.m_navigation.MGD_Matrix );
    VOGMatrix3x3Identity( &m_Settings.m_navigation.OAR_Matrix );

    // Create Buffers
	if (( m_pBufferDark = (int *)malloc(m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY * sizeof(UINT32))) == NULL)
		    	throw Exception("CamOAR Error 0");

	if (( m_pBuffer0 = (UINT16 *)malloc(m_Settings.m_dwDataSize)) == NULL)
		    	throw Exception("CamOAR Error 1");


	if (( m_pBuffer1 = (UINT16 *)malloc(m_Settings.m_dwDataSize)) == NULL)
		    	throw Exception("CamOAR Error 2");

    // Create Buffers for Arrived Data
	if (( m_Settings.m_pBufferArrived = (UINT16 *)malloc( 1024 * 1024 )) == NULL)
		    	throw Exception("CamOAR Error 3");


	m_Settings.m_pBuffer			= m_pBuffer0;	// Current work buffer
	m_Settings.m_bBuff				= 0;			// Current work buffer index

    m_bBuffer0Blocked               = false;
    m_bBuffer1Blocked               = false;

    // Create Bitmaps
	HDC	hDC = CreateCompatibleDC( NULL );

	// Prepare to create a bitmap
	DWORD*      pBitmapBits;
	BITMAPINFO bmi;
	ZeroMemory( &bmi.bmiHeader, sizeof(BITMAPINFOHEADER) );
    bmi.bmiHeader.biSize        = sizeof(BITMAPINFOHEADER);
    bmi.bmiHeader.biWidth       =  (int)m_Settings.m_dwDataSizeX;
    bmi.bmiHeader.biHeight      = -(int)m_Settings.m_dwDataSizeY;
    bmi.bmiHeader.biPlanes      = 1;
    bmi.bmiHeader.biCompression = BI_RGB;	// An uncompressed format.
    bmi.bmiHeader.biBitCount    = 32;

	// The CreateDIBSection function creates a device-independent bitmap (DIB) that applications can write to directly.
    // The function gives you a pointer to the location of the bitmap's bit values.
    // You can supply a handle to a file mapping object that the function will use to create the bitmap,
    // or you can let the operating system allocate the memory for the bitmap.

    m_hbmBitmap = CreateDIBSection( hDC, &bmi, DIB_RGB_COLORS, (void**)&pBitmapBits, NULL, 0 );

    // Gistogramm
	ZeroMemory( &bmi.bmiHeader, sizeof(BITMAPINFOHEADER) );
    bmi.bmiHeader.biSize        = sizeof(BITMAPINFOHEADER);
    bmi.bmiHeader.biWidth       =  (int)m_Settings.m_dwGistSizeX;
    bmi.bmiHeader.biHeight      = -(int)m_Settings.m_dwGistSizeY;
    bmi.bmiHeader.biPlanes      = 1;
    bmi.bmiHeader.biCompression = BI_RGB;	// An uncompressed format.
    bmi.bmiHeader.biBitCount    = 32;

    m_hbmGistogramm = CreateDIBSection( hDC, &bmi, DIB_RGB_COLORS, (void**)&pBitmapBits, NULL, 0 );

    DeleteDC( hDC );

    // Set Pointers
    GetObject((HGDIOBJ)m_hbmBitmap, sizeof(BITMAP), (LPVOID)&m_bitmap);
    GetObject((HGDIOBJ)m_hbmGistogramm, sizeof(BITMAP), (LPVOID)&m_bitmapGistogramm);

    // Add Objects
	if (m_D3DApp != NULL)
    	{
		    m_pD3DBitmap = m_D3DApp->AddObject("Данные CamOAR", m_hbmBitmap, m_Settings.m_dwDataSizeX, m_Settings.m_dwDataSizeY);
		    m_pD3DBitmap->SetAlpha(255);
            m_pD3DBitmap->SetOriginalScale(true);

		    m_pD3DbitmapGistogramm = m_D3DApp->AddObject("Гистограмма CamOAR", m_hbmGistogramm, m_Settings.m_dwGistSizeX, m_Settings.m_dwGistSizeY);
		    m_pD3DbitmapGistogramm->SetAlpha(100);
            m_pD3DbitmapGistogramm->SetSizeFlags(4);
		}
}

//-----------------------------------------------------------------------------
// Name: InvalidateData()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::InvalidateData()
{
    // Test
    if ( m_avi )
        {
            CloseAvi( m_avi );
            m_avi = NULL;
        }

    // Delete Objects
	if ((bVideoSystemInited)&&(m_D3DApp != NULL))
    	{
			m_D3DApp->DeleteObject(m_pD3DBitmap);
            m_D3DApp->DeleteObject(m_pD3DbitmapGistogramm);
		}


  	free(m_pBuffer0);
   	free(m_pBuffer1);
    free(m_pBufferDark);

    free(m_Settings.m_pBufferArrived);

    DeleteObject( m_hbmBitmap );
    DeleteObject( m_hbmGistogramm );
}

//-----------------------------------------------------------------------------
// Name: SetUpData()
// Desc: Set up data for processing
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetUpData(void * pBufferIN, UINT32 dwDataSize, UINT32 uiReadBufferNumber)
{
    FRAMEHEADER3 * pHead = (FRAMEHEADER3 *)pBufferIN;

    switch ( pHead->ASMHeader.MesId )
    {
        case MID_PARAMREQ_FLASH_NEW, MID_PARAMREQ_EEPROM_NEW:
            // Пришел ответ на запрос, копируем ответ на запрос в буфер
            memcpy(m_Settings.m_pBufferArrived, (void *)(DWORD)pBufferIN, dwDataSize );
            m_Settings.bDataArrived = true;
        break;


        case 0x5:
            // Если пришел статус
            memcpy(&m_Settings.m_OARStatus, (void *)((DWORD)pBufferIN + sizeof(CHBUFF) + sizeof(ASMHEADER) + sizeof(UINT64)), sizeof(TOARStatus));
        break;

        case 0x50:
            // Пришел кадр
            // Копировать данные навигационной информации
            memcpy(&m_Settings.m_navigationIN, (void *)((DWORD)pBufferIN + sizeof(CHBUFF) + sizeof(ASMHEADER) + sizeof(UINT64) + sizeof(TBITMAPFILEHEADER) + sizeof(TBITMAPINFOHEADER2) ), sizeof(TNavigation));

            // Копировать кадр
        	if (m_Settings.m_bBuff)
            	{
                    if (!m_bBuffer0Blocked)
            			memcpy(m_pBuffer0, (void *)((DWORD)pBufferIN + sizeof(FRAMEHEADER3)), m_Settings.m_dwDataSize);
        		}
        	else
            	{
                    if (!m_bBuffer1Blocked)
                        memcpy(m_pBuffer1, (void *)((DWORD)pBufferIN + sizeof(FRAMEHEADER3)), m_Settings.m_dwDataSize);
                }

        	m_bHasData = true;
        break;
    }
}

//-----------------------------------------------------------------------------
// Name: SetUpSettings()
// Desc: Set up data for processing
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetUpSettings()
{

	TForm_Process_CAMOAR * ProcessingFormDlg;
	ProcessingFormDlg = new TForm_Process_CAMOAR(Application);		// construct the Dlg box
	try
		{
            ProcessingFormDlg->CAMOARSettings = &m_Settings;
            ProcessingFormDlg->ShowModal();
    	}
	__finally
    	{
		   	delete ProcessingFormDlg;
        }
}

//-----------------------------------------------------------------------------
// Name: SetUpVideo()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetUpVideo()
{
	TForm_CAMOAR* Form_VideoOutOptions;
	Form_VideoOutOptions = new TForm_CAMOAR(Application);		// construct the Dlg box
	try
		{
			Form_VideoOutOptions->D3DBitmapGistogram	= m_pD3DbitmapGistogramm;
            Form_VideoOutOptions->D3DBitmap				= m_pD3DBitmap;
            Form_VideoOutOptions->CAMOARSettings		= &m_Settings;
            Form_VideoOutOptions->D3DApplication        = m_D3DApp;
            Form_VideoOutOptions->ShowModal();
    	}
	__finally
    	{
		   	delete Form_VideoOutOptions;
            return;
        }
}

//-----------------------------------------------------------------------------
// Name: SetUpControl()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetUpControl()
{
	TForm_Control_CAMOAR * Form_Control;
	Form_Control = new TForm_Control_CAMOAR(Application);		// construct the Dlg box
	try
		{
            Form_Control->CAMOARSettings		= &m_Settings;
            Form_Control->ShowModal();
    	}
	__finally
    	{
		   	delete Form_Control;
            return;
        }
}

//-----------------------------------------------------------------------------
// Name: OnKeyDown()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall  TCamCAMOAR::OnKeyDown(WORD &Key, TShiftState Shift )
{
    switch (Key)
    {
        case Key_G :
            // Режим отображения гистограммы
            m_Settings.m_Reg = 0;   // Гистограмма

		    m_pD3DbitmapGistogramm->SetAlpha(100);
            m_pD3DbitmapGistogramm->SetVisible(true);
            m_pD3DbitmapGistogramm->SetSizeFlags(4);
        break;

        case Key_N :
            // Режим отображения навигационных параметров
            m_Settings.m_Reg = 1;   // Навигационные параметры

            m_Settings.m_Nav += 1;
            if ( m_Settings.m_Nav > 2)
                m_Settings.m_Nav = 0;

		    m_pD3DbitmapGistogramm->SetAlpha(100);
            m_pD3DbitmapGistogramm->SetVisible(true);
            m_pD3DbitmapGistogramm->SetSizeFlags(4);
        break;

        case Key_S :
            // Режим отображения статусной информации
            m_Settings.m_Reg = 2;   // Статус

            m_Settings.m_CurStat += 1;
            if ( m_Settings.m_CurStat > 2)
                m_Settings.m_CurStat = 0;

		    m_pD3DbitmapGistogramm->SetAlpha(100);
            m_pD3DbitmapGistogramm->SetVisible(true);
            m_pD3DbitmapGistogramm->SetSizeFlags(4);
        break;

        case Key_I :
            // Режим отображения данных общего характера (есть ли запись в файл, количество записанных файлов и т.д.)
            m_Settings.m_Reg = 3;   // Количество кадров, записанных в AVI и режим
        break;

        // TEST VideoCapture
        // Включение / отключение записи видео
        case Key_A :
            m_Settings.m_bCaptureOn = !m_Settings.m_bCaptureOn;
        break;

        case Key_V :
            m_bFirstTime = true;
            m_Settings.m_bCaptureOn = !m_Settings.m_bCaptureOn;
        break;
    }
}


//-----------------------------------------------------------------------------
// Name: OnKeyUp()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall  TCamCAMOAR::OnKeyUp(WORD &Key, TShiftState Shift )
{
}


//-----------------------------------------------------------------------------
// Name: OnKeyPress()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall  TCamCAMOAR::OnKeyPress(char &Key)
{
}


//-----------------------------------------------------------------------------
// Name: MakeDumps()
// Desc: TEST
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::MakeDumps()
{
    static UINT32 dwDumpCount = 0;
    char szFileName[MAXFILE + 4];
    AnsiString asData;
    int iFileHandle;
    char * iLength;

    if (m_Settings.m_swDumpCount > 0)
        {
            m_Settings.m_swDumpCount--;
        }
    else
        {
            m_Settings.m_bMakeDumps = false;
            return;        
        }

    iFileHandle = FileCreate(IntToStr(dwDumpCount) + ".txt");
    FileSeek(iFileHandle,0,0);

//    DWORD dwBufStep = m_Settings.m_dwDataSizeX;
    UINT16 * pBuf = (UINT16 *)m_Settings.m_pBuffer;

    // Записать начальный пробел
    iLength = " ";
    FileWrite(iFileHandle, iLength, 1);

	for (int j = 0; j < (int)(m_Settings.m_dwDataSizeX); j++)
		{
        	UINT16 * pCurrBuff   = pBuf;

			for (int i = 0; i < (int)(m_Settings.m_dwDataSizeY / 4 ); i++)
				{
                    UINT16 iTmp = * pCurrBuff++;
                    asData = "0x" + IntToHex(iTmp, 4);
                    FileWrite(iFileHandle, asData.c_str(), 6);
                    iLength = " ";
                    FileWrite(iFileHandle, iLength, 1);

                    iTmp = * pCurrBuff++;
                    asData = "0x" + IntToHex(iTmp, 4);
                    FileWrite(iFileHandle, asData.c_str(), 6);
                    iLength = " ";
                    FileWrite(iFileHandle, iLength, 1);

                    iTmp = * pCurrBuff++;
                    asData = "0x" + IntToHex(iTmp, 4);
                    FileWrite(iFileHandle, asData.c_str(), 6);
                    iLength = " ";
                    FileWrite(iFileHandle, iLength, 1);

                    iTmp = * pCurrBuff++;
                    asData = "0x" + IntToHex(iTmp, 4);
                    FileWrite(iFileHandle, asData.c_str(), 6);
                    iLength = " ";
                    FileWrite(iFileHandle, iLength, 1);
				}
            iLength = " \r\n ";
            FileWrite(iFileHandle, iLength, sizeof(iLength));

            pBuf += m_Settings.m_dwDataSizeY ;
		}

    FileClose(iFileHandle);
}

//-----------------------------------------------------------------------------
// Name: GetMinMax()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::GetMinMax()
{
    UINT16 * pCurrPointer = (UINT16 *)m_Settings.m_pBuffer;
    // Количество пикселей в кадре
    DWORD dwPixelCount = m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY;

	int dwAverage			= 0;            // Переменная для набора среднего
	UINT16 wMax				= 0x0;
	UINT16 wMin             = 0xFFFF;

    UINT16 pCurrValue;
    for (int i = 0 ; i < dwPixelCount; i++)
        {
            pCurrValue = * pCurrPointer++;

            // Набор среднего
	        dwAverage += pCurrValue;

            // Максимальное
            if ( pCurrValue > wMax)
            	{
					wMax = pCurrValue;
                }

            // Минимальное
            if ( pCurrValue < wMin)
            	{
					wMin = pCurrValue;
                }
        }

	// Среднее
    m_Settings.m_fAverage = dwAverage / dwPixelCount ;
	// Максимальное значение
    m_Settings.m_wMaxVal = wMax;
    // Минимальное значение
    m_Settings.m_wMinVal = wMin;
}

//-----------------------------------------------------------------------------
// Name: BlockBuffer()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::BlockBuffer()
{
	if (m_Settings.m_bBuff)
        m_bBuffer1Blocked = true;
    else
        m_bBuffer0Blocked = true;
}

//-----------------------------------------------------------------------------
// Name: UnblockBuffer()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::UnblockBuffer()
{
	if (m_Settings.m_bBuff)
        m_bBuffer1Blocked = false;
    else
        m_bBuffer0Blocked = false;
}

//-----------------------------------------------------------------------------
// Name: ProcessData()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ProcessData()
{
//	m_bHasData = false;

    // Block Current Work Buffer
    BlockBuffer();

    // Включить/отключить запись видео
    SetCaptureVideo();

    // Записать текстовые дампы в файл
    if (m_Settings.m_bMakeDumps)
        MakeDumps();

    // Get Min, Max, Average
    GetMinMax();

    // Create Dark Frame
   	if (m_Settings.m_bDarkFrame)
		SetDarkFrame();

    // Subtract Dark Frame
	if (m_Settings.m_bSubDarkFrame && m_Settings.m_bDarkFrameInited)
		SubDarkFrame();


    // Information
    switch (m_Settings.m_Reg)
    {
        case 0:
            // Gistogramm
        	if (m_Settings.m_bGistogramm)
            	{
                    SetGistogramm();
                    MakeGistogramm();
                }
        break;

        case 1:
            // Navigation  MGD
            ShowNavigationParams();
        break;

        case 2:
            // Status
            ShowStatus();
        break;

        case 3:
            // Capture
            ShowCaptureParams();
        break;
    }
    if ( m_pD3DbitmapGistogramm != NULL )
        m_pD3DbitmapGistogramm->SetUpdateData(true);


    // Correct Broken Pixels
    if (m_Settings.m_bFindBrokenPixels)
    {
        ClearBadPixelsList();
        GetBadPixels();
        m_Settings.m_bFindBrokenPixels = false;        
    }

    if (m_Settings.m_bBrokenPixels)
    {
        CorrectBadPixels();
    }

    // Draw Data to Bitmap
    // Contrast
    switch ( m_Settings.m_dwContrast )
    {
        // Контрастирование отключено
        case 0 :
        SetData();
        break;

        // Контрастирование по фиксированным уровням
        case 1 :
        m_Settings.m_wMinCutLev = m_Settings.m_wMinLev;
        m_Settings.m_wMaxCutLev = m_Settings.m_wMaxLev;
        SetDataLevels();
        break;

        // Автоконтрастирование
        case 2 :
        m_Settings.m_wMinCutLev = m_Settings.m_wMinVal;
        m_Settings.m_wMaxCutLev = m_Settings.m_wMaxVal;
        SetDataLevels();
        break;
    }

    // TEST !!!
    //VisualizeBadPixels();

	if (m_pD3DBitmap != NULL)
        m_pD3DBitmap->SetUpdateData(true);

    // Copy TEST
    if ( m_pD3DBitmap->IsVideoOutput() )
        {
            hr_st = m_D3DApp->CopyBitmapToInnerSurface( m_pD3DBitmap, true );
        }

    // CAPTURE TEST!!!
    CaptureVideoToAVI();    

    // Block Current Work Buffer
    UnblockBuffer();

	FlipBuffers();

    m_bHasData = false;
}

//-----------------------------------------------------------------------------
// Name: FlipBuffers()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::FlipBuffers()
{
	m_Settings.m_bBuff = !m_Settings.m_bBuff;

	if (m_Settings.m_bBuff)
    	{
			m_Settings.m_pBuffer = m_pBuffer1;
        }
	else
    	{
			m_Settings.m_pBuffer = m_pBuffer0;
        }
}

//-----------------------------------------------------------------------------
// Name: SetDataLevels()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetDataLevels()
{
    TVideoData1 * pBuf = (TVideoData1 *)m_Settings.m_pBuffer;

    UINT16 wMin = m_Settings.m_wMinCutLev;
    UINT16 wMax = m_Settings.m_wMaxCutLev;
	UINT8 Bits = 8;

    DWORD dwBmpStep = m_bitmap.bmWidthBytes / 4;    // Шаг инкремента строк в BMP
	DDRGBA * pBmpStr1 = (DDRGBA *)m_bitmap.bmBits;  // Указатель на первую строку для записи в BMP
	DDRGBA * pBmpStr2 = pBmpStr1 + dwBmpStep;       // Указатель на вторую строку для записи в BMP
	DDRGBA * pBmpStr3 = pBmpStr2 + dwBmpStep;       // Указатель на третью строку для записи в BMP
	DDRGBA * pBmpStr4 = pBmpStr3 + dwBmpStep;       // Указатель на четвертую строку для записи в BMP
    // Test
	DDRGBA * pBmpStr5 = pBmpStr4 + dwBmpStep;       // Указатель на пятую строку для записи в BMP
	DDRGBA * pBmpStr6 = pBmpStr5 + dwBmpStep;       // Указатель на шестую строку для записи в BMP
	DDRGBA * pBmpStr7 = pBmpStr6 + dwBmpStep;       // Указатель на седьмую строку для записи в BMP
	DDRGBA * pBmpStr8 = pBmpStr7 + dwBmpStep;       // Указатель на восьмую строку для записи в BMP

    for (int i = 0; i < m_Settings.m_dwDataSizeX ; i++)
        {
           	DDRGBA * pCurrBmpStr1 = pBmpStr1;
           	DDRGBA * pCurrBmpStr2 = pBmpStr2;
           	DDRGBA * pCurrBmpStr3 = pBmpStr3;
           	DDRGBA * pCurrBmpStr4 = pBmpStr4;
            // Test
           	DDRGBA * pCurrBmpStr5 = pBmpStr5;
           	DDRGBA * pCurrBmpStr6 = pBmpStr6;
           	DDRGBA * pCurrBmpStr7 = pBmpStr7;
           	DDRGBA * pCurrBmpStr8 = pBmpStr8;
        	for (int j = 0; j < (m_Settings.m_dwDataSizeY / 8); j++)
        		{
                    UINT16 iTmp;

                    //iTmp = GetValue((UINT16)pBuf->uiADC1);
                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC1, wMin, wMax  );//  >> 8;
					pCurrBmpStr1->blue  	= iTmp;
					pCurrBmpStr1->green     = iTmp;
					pCurrBmpStr1->red 	    = iTmp;

                    //iTmp = GetValue((UINT16)pBuf->uiADC2);
                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC2, wMin, wMax  );//  >> 8;
					pCurrBmpStr2->blue  	= iTmp;
					pCurrBmpStr2->green     = iTmp;
					pCurrBmpStr2->red 	    = iTmp;

                    //iTmp = GetValue((UINT16)pBuf->uiADC3);
                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC3, wMin, wMax  );//  >> 8;
					pCurrBmpStr3->blue  	= iTmp;
					pCurrBmpStr3->green     = iTmp;
					pCurrBmpStr3->red 	    = iTmp;

                    //iTmp = GetValue((UINT16)pBuf->uiADC4);
                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC4, wMin, wMax  );//  >> 8;
					pCurrBmpStr4->blue  	= iTmp;
					pCurrBmpStr4->green     = iTmp;
					pCurrBmpStr4->red 	    = iTmp;

                    // Test
                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC5, wMin, wMax  );//  >> 8;
					pCurrBmpStr5->blue  	= iTmp;
					pCurrBmpStr5->green     = iTmp;
					pCurrBmpStr5->red 	    = iTmp;

                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC6, wMin, wMax  );//  >> 8;
					pCurrBmpStr6->blue  	= iTmp;
					pCurrBmpStr6->green     = iTmp;
					pCurrBmpStr6->red 	    = iTmp;

                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC7, wMin, wMax  );//  >> 8;
					pCurrBmpStr7->blue  	= iTmp;
					pCurrBmpStr7->green     = iTmp;
					pCurrBmpStr7->red 	    = iTmp;

                    iTmp = GetContrastValueOAR( (UINT16)pBuf->uiADC8, wMin, wMax  );//  >> 8;
					pCurrBmpStr8->blue  	= iTmp;
					pCurrBmpStr8->green     = iTmp;
					pCurrBmpStr8->red 	    = iTmp;

                    pBuf++;

                    pCurrBmpStr1 = pCurrBmpStr8 + dwBmpStep;
                    pCurrBmpStr2 = pCurrBmpStr1 + dwBmpStep;
                    pCurrBmpStr3 = pCurrBmpStr2 + dwBmpStep;
                    pCurrBmpStr4 = pCurrBmpStr3 + dwBmpStep;
                    pCurrBmpStr5 = pCurrBmpStr4 + dwBmpStep;
                    pCurrBmpStr6 = pCurrBmpStr5 + dwBmpStep;
                    pCurrBmpStr7 = pCurrBmpStr6 + dwBmpStep;
                    pCurrBmpStr8 = pCurrBmpStr7 + dwBmpStep;
                }
			pBmpStr1++;
			pBmpStr2++;
			pBmpStr3++;
			pBmpStr4++;
            // Test
			pBmpStr5++;
			pBmpStr6++;
			pBmpStr7++;
			pBmpStr8++;
        }
}


//-----------------------------------------------------------------------------
// Name: SetData()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetData()
{
    TVideoData1 * pBuf = (TVideoData1 *)m_Settings.m_pBuffer;

    DWORD dwBmpStep = m_bitmap.bmWidthBytes / 4;    // Шаг инкремента строк в BMP
	DDRGBA * pBmpStr1 = (DDRGBA *)m_bitmap.bmBits;  // Указатель на первую строку для записи в BMP
	DDRGBA * pBmpStr2 = pBmpStr1 + dwBmpStep;       // Указатель на вторую строку для записи в BMP
	DDRGBA * pBmpStr3 = pBmpStr2 + dwBmpStep;       // Указатель на третью строку для записи в BMP
	DDRGBA * pBmpStr4 = pBmpStr3 + dwBmpStep;       // Указатель на четвертую строку для записи в BMP
    // Test
	DDRGBA * pBmpStr5 = pBmpStr4 + dwBmpStep;       // Указатель на пятую строку для записи в BMP
	DDRGBA * pBmpStr6 = pBmpStr5 + dwBmpStep;       // Указатель на шестую строку для записи в BMP
	DDRGBA * pBmpStr7 = pBmpStr6 + dwBmpStep;       // Указатель на седьмую строку для записи в BMP
	DDRGBA * pBmpStr8 = pBmpStr7 + dwBmpStep;       // Указатель на восьмую строку для записи в BMP

    for (int i = 0; i < m_Settings.m_dwDataSizeX ; i++)
        {
           	DDRGBA * pCurrBmpStr1 = pBmpStr1;
           	DDRGBA * pCurrBmpStr2 = pBmpStr2;
           	DDRGBA * pCurrBmpStr3 = pBmpStr3;
           	DDRGBA * pCurrBmpStr4 = pBmpStr4;
            // Test
           	DDRGBA * pCurrBmpStr5 = pBmpStr5;
           	DDRGBA * pCurrBmpStr6 = pBmpStr6;
           	DDRGBA * pCurrBmpStr7 = pBmpStr7;
           	DDRGBA * pCurrBmpStr8 = pBmpStr8;
        	for (int j = 0; j < (m_Settings.m_dwDataSizeY / 8); j++)
        		{
                    UINT16 iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC1)) >> 8;
					pCurrBmpStr1->blue  	= iTmp;
					pCurrBmpStr1->green     = iTmp;
					pCurrBmpStr1->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC2)) >> 8;
					pCurrBmpStr2->blue  	= iTmp;
					pCurrBmpStr2->green     = iTmp;
					pCurrBmpStr2->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC3)) >> 8;
					pCurrBmpStr3->blue  	= iTmp;
					pCurrBmpStr3->green     = iTmp;
					pCurrBmpStr3->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC4)) >> 8;
					pCurrBmpStr4->blue  	= iTmp;
					pCurrBmpStr4->green     = iTmp;
					pCurrBmpStr4->red 	    = iTmp;

                    // Test
                    iTmp = ((UINT16)(pBuf->uiADC5)) >> 8;
					pCurrBmpStr5->blue  	= iTmp;
					pCurrBmpStr5->green     = iTmp;
					pCurrBmpStr5->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC6)) >> 8;
					pCurrBmpStr6->blue  	= iTmp;
					pCurrBmpStr6->green     = iTmp;
					pCurrBmpStr6->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC7)) >> 8;
					pCurrBmpStr7->blue  	= iTmp;
					pCurrBmpStr7->green     = iTmp;
					pCurrBmpStr7->red 	    = iTmp;

                    iTmp = ((UINT16)(pBuf->uiADC8)) >> 8;
					pCurrBmpStr8->blue  	= iTmp;
					pCurrBmpStr8->green     = iTmp;
					pCurrBmpStr8->red 	    = iTmp;

                    pBuf++;

                    pCurrBmpStr1 = pCurrBmpStr8 + dwBmpStep;
                    pCurrBmpStr2 = pCurrBmpStr1 + dwBmpStep;
                    pCurrBmpStr3 = pCurrBmpStr2 + dwBmpStep;
                    pCurrBmpStr4 = pCurrBmpStr3 + dwBmpStep;
                    pCurrBmpStr5 = pCurrBmpStr4 + dwBmpStep;
                    pCurrBmpStr6 = pCurrBmpStr5 + dwBmpStep;
                    pCurrBmpStr7 = pCurrBmpStr6 + dwBmpStep;
                    pCurrBmpStr8 = pCurrBmpStr7 + dwBmpStep;
                }
			pBmpStr1++;
			pBmpStr2++;
			pBmpStr3++;
			pBmpStr4++;

            // Test
			pBmpStr5++;
			pBmpStr6++;
			pBmpStr7++;
			pBmpStr8++;
        }
}



//-----------------------------------------------------------------------------
// Name: MakeGistogramm()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::MakeGistogramm()
{
	HDC hDC;
	HGDIOBJ hbmOld = NULL;
    HPEN	PenData = NULL;
    HPEN	PenMinLevel = NULL;
    HPEN	PenMaxLevel = NULL;
    HPEN	OldPen = NULL;

	if (m_hbmGistogramm == NULL)
    	return;

	// Clear last bmp
	ZeroMemory(m_bitmapGistogramm.bmBits, m_bitmapGistogramm.bmWidthBytes * m_bitmapGistogramm.bmHeight);

    // Диапазон 16 бит
    DWORD dwGistSize = (0xFFFF >> m_Settings.m_dwShift) + 1;
    FLOAT fCoeffX = (FLOAT)( m_bitmapGistogramm.bmWidth ) / (FLOAT)dwGistSize;
    FLOAT fCoeffY = 0;
    if (m_Settings.m_dwMax > 0)
		fCoeffY = (FLOAT)(m_bitmapGistogramm.bmHeight - 10) / (FLOAT)m_Settings.m_dwMax;

	hDC = CreateCompatibleDC( NULL );
	hbmOld = SelectObject( hDC, m_hbmGistogramm );

	// Create Pen for drawing
	PenData = CreatePen(PS_SOLID, 1, RGB(255, 255, 255));
	// Set Pen and save old pen object
	OldPen = SelectObject(hDC, PenData);
   	// Set initial position
    MoveToEx(hDC, 0, m_bitmapGistogramm.bmHeight, NULL);

    SetBkColor(hDC, RGB(0, 255, 0));
    SetBkMode(hDC, TRANSPARENT);
    SetTextColor(hDC, RGB(255, 255, 255));

    float fLeftRect	= 0;
	for (int i = 0; i < dwGistSize; i++)
    	{
            float fTopRect		= m_bitmapGistogramm.bmHeight - ((FLOAT)m_dwaGistogramm[i] * fCoeffY);
            float fRightRect	= fLeftRect + fCoeffX;
            float fBottomRect	= m_bitmapGistogramm.bmHeight;

		    int nLeftRect	= (int)fLeftRect;	 	// x-coord. of bounding rectangle's upper-left corner
		    int nTopRect    = (int)fTopRect;		// y-coord. of bounding rectangle's upper-left corner
		    int nRightRect	= (int)fRightRect;	 	// x-coord. of bounding rectangle's lower-right corner
		    int nBottomRect = (int)fBottomRect;		// y-coord. of bounding rectangle's lower-right corner
            Rectangle(hDC, nLeftRect, nTopRect, nRightRect, nBottomRect);

            fLeftRect = fRightRect;
        }

    // Draw Levels
    FLOAT fCoeffLevel = (FLOAT)( m_bitmapGistogramm.bmWidth ) / 0xFFFF;
	// Create Pen for drawing Min Level
	PenMinLevel = CreatePen(PS_SOLID, 1, RGB(255, 0, 0));
	// Set Pen
    SelectObject(hDC, PenMinLevel);
   	// Set initial position
    MoveToEx(hDC, (int)(fCoeffLevel * m_Settings.m_wMinCutLev), m_bitmapGistogramm.bmHeight, NULL);
    // Draw Level Line
    LineTo(hDC, (int)(fCoeffLevel * m_Settings.m_wMinCutLev), 0);

	// Create Pen for drawing Max Level
	PenMaxLevel = CreatePen(PS_SOLID, 1, RGB(0, 0, 255));
	// Set Pen
    SelectObject(hDC, PenMaxLevel);
   	// Set initial position
    MoveToEx(hDC, (int)(fCoeffLevel * m_Settings.m_wMaxCutLev), m_bitmapGistogramm.bmHeight, NULL);
    // Draw Level Line
    LineTo(hDC, (int)(fCoeffLevel * m_Settings.m_wMaxCutLev), 0);

    UINT8 ucOffset = 10;

    if (m_Settings.m_bAverage)
        {
            // Send some text out into the world
            AnsiString Astr = "Среднее : " + FloatToStrF(m_Settings.m_fAverage, ffFixed, 6, 4 );
            char* cp = new char[ Astr.Length() + 1 ];
            strcpy( cp, Astr.c_str() );
            TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
            delete[] cp;
            ucOffset += 20;
        }

    if (m_Settings.m_bMin)
        {
            // Send some text out into the world
            AnsiString Astr = "MIN : " + IntToStr(m_Settings.m_wMinVal);
            char* cp = new char[ Astr.Length() + 1 ];
            strcpy( cp, Astr.c_str() );
            TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
            delete[] cp;
            ucOffset += 20;
        }

    if (m_Settings.m_bMax)
        {
            // Send some text out into the world
            AnsiString Astr = "MAX : " + IntToStr(m_Settings.m_wMaxVal);
            char* cp = new char[ Astr.Length() + 1 ];
            strcpy( cp, Astr.c_str() );
            TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
            delete[] cp;
            ucOffset += 20;
        }

	SelectObject( hDC, OldPen);
    SelectObject( hDC, hbmOld );
    DeleteObject( PenData );
    DeleteObject( PenMinLevel );
    DeleteObject( PenMaxLevel );
    DeleteDC( hDC );
}


//-----------------------------------------------------------------------------
// Name: SetGistogramm()
// Desc: Black and White
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetGistogramm()
{
	m_Settings.m_dwMax		= 0;
	m_Settings.m_dwMin		= 0xFFFF;
	m_Settings.m_dwMaxIndex	= 0;
	m_Settings.m_dwMinIndex	= 0;

	UINT16 * pCurrPointer = (UINT16 *)m_Settings.m_pBuffer;
    // Диапазон 16 бит
    UINT32 dwDiagrammSize = (0xFFFF >> m_Settings.m_dwShift) + 1;
    // Очистка диаграммы
   	memset(&m_dwaGistogramm, 0, dwDiagrammSize * sizeof(DWORD));

    UINT16 pCurrValue;
    // Построение гистограммы
	for (int i = 0; i < (int)(m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY); i++)
		{
        	pCurrValue = *pCurrPointer++;
			m_dwaGistogramm[ pCurrValue >> m_Settings.m_dwShift ]++;
		}

    // Количество пикселей в кадре
//    DWORD dwPixelCount = m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY;
    for (int i = 0; i < (int)dwDiagrammSize; i++)
		{
		    // Get maximum value
			if (m_dwaGistogramm[i] > m_Settings.m_dwMax)
            	{
                	 m_Settings.m_dwMax			= m_dwaGistogramm[i];
                     m_Settings.m_dwMaxIndex	= i;
                }
		    // Get minimum value
			if (m_dwaGistogramm[i] < m_Settings.m_dwMin)
            	{
                     m_Settings.m_dwMin			= m_dwaGistogramm[i];
                     m_Settings.m_dwMinIndex	= i;
                }

            // Определение нормированной вероятности появления пикселя
//            m_faGistogramm[i] = (float)(m_dwaGistogramm[i]) / ( dwPixelCount );
        }
}

//-----------------------------------------------------------------------------
// Name: ClearDarkFrame()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ClearDarkFrame()
{
	memset(m_pBufferDark, 0, m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY * sizeof(UINT32));
}

//-----------------------------------------------------------------------------
// Name: SetDarkBuffer()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetDarkBuffer()
{
	UINT16 * pPointer1		= (UINT16 *)m_Settings.m_pBuffer;
	int * pCurrentStr1      = &m_pBufferDark[0];

	for (int j = 0; j < m_Settings.m_dwDataSizeY; j++)
    	{
			for (int i = 0; i < m_Settings.m_dwDataSizeX; i++)
	        	{
                   (* pCurrentStr1) += (* pPointer1);
                   pCurrentStr1++;
                   pPointer1++;
	            }
		}
}

//-----------------------------------------------------------------------------
// Name: MakeDarkFrame()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::MakeDarkFrame()
{
	int * pPointer1 = &m_pBufferDark[0];
	for (int i = 0; i < m_Settings.m_dwDataSizeY; i++)
    	{
			for (int j = 0; j < m_Settings.m_dwDataSizeX; j++)
	        	{
                	// Нормализуем буфер
                    (* pPointer1) >>= 6; // Деление на 64
                    pPointer1++;
	            }
		}
}

//-----------------------------------------------------------------------------
// Name: SetDarkFrame()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetDarkFrame()
{
	// Очистка предыдущего кадра
	if (m_Settings.m_dwFrameAdded == 0)
		ClearDarkFrame();

	// Добавить очередной буфер
	SetDarkBuffer();

	// Увеличить количество кадров
	m_Settings.m_dwFrameAdded++;

	if (m_Settings.m_dwFrameAdded == 64)
		{
			MakeDarkFrame();
			m_Settings.m_bDarkFrame			= false;
			m_Settings.m_bDarkFrameInited	= true;
			m_Settings.m_dwFrameAdded		= 0;
		}
}

//-----------------------------------------------------------------------------
// Name: SubDarkFrame()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SubDarkFrame()
{
	UINT16 * pPointer1		= &(((UINT16 *)m_Settings.m_pBuffer)[0]);		// Указатель на буфер, пришедший
	int * pPointer2		    = &m_pBufferDark[0];							// Указатель на буфер темнового кадра
    int iTmp;
	for (int i = 0; i < (int)m_Settings.m_dwDataSizeY; i++)
    	{
			for (int j = 0; j < m_Settings.m_dwDataSizeX; j++)
	        	{
                    iTmp = pPointer1[j] - pPointer2[j];
                    if (iTmp > 0)
                    	{
	                        pPointer1[j] = iTmp;
                        }
					else
                    	{
                        	pPointer1[j] = 0;
                        }
	            }
			pPointer1 += m_Settings.m_dwDataSizeX;
            pPointer2 += m_Settings.m_dwDataSizeX;
		}
}


//-----------------------------------------------------------------------------
// Bad Pixels Corrections
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
// Name: ClearBadPixelsList()
// Desc: Clear Bad Pixels List
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ClearBadPixelsList()
{
    for (int i = 0; i < m_Settings.m_dwBadPixelsCount; i++)
        {
            if (m_BadPixels[i] != NULL)
                {
                    delete m_BadPixels[i];
                }
        }

    m_Settings.m_dwBadPixelsCount = 0;
}

//-----------------------------------------------------------------------------
// Name: CheckBadPixelsList()
// Desc:
//-----------------------------------------------------------------------------
bool __fastcall TCamCAMOAR::CheckBadPixelsList(int x, int y)
{
    DWORD dwIndexs = y * m_Settings.m_dwDataSizeX + x;
    for (int i = 0; i < m_Settings.m_dwBadPixelsCount; i++)
        {
            if (m_BadPixels[i]->dwIndexs == dwIndexs)
                {
                    return true;
                }
        }
        
    return false;
}

//-----------------------------------------------------------------------------
// Name: GetBadPixels()
// Desc: Get Bad Pixels Count
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::GetBadPixels()
{
    UINT16 * pPointer1		= (UINT16 *)m_Settings.m_pBuffer;		// Указатель на пришедший буфер

    for (int i = 0; i < (int)(m_Settings.m_dwDataSizeX * m_Settings.m_dwDataSizeY); i++)
        if (( pPointer1[i] <= m_Settings.m_wMinCutLev ) || ( pPointer1[i] >= m_Settings.m_wMaxCutLev ))
            {
                if (m_Settings.m_dwBadPixelsCount < MAX_BAD_PIXELS)
                    {
                        // Make new description
                        TBadPixelDesc1 * pBadPixelDesc = new TBadPixelDesc1;
                        // Set bad pixel index
                        pBadPixelDesc->dwIndexs = i;
                        // Set bad pixel coords
                        pBadPixelDesc->tpCoords.y = i / m_Settings.m_dwDataSizeX;
                        pBadPixelDesc->tpCoords.x = i - pBadPixelDesc->tpCoords.y * m_Settings.m_dwDataSizeX;
                        pBadPixelDesc->dwNeigbourCount = 0;
                        // Set bad pixel
                        m_BadPixels[m_Settings.m_dwBadPixelsCount] = pBadPixelDesc;
                        // Increase bad pixels counter
                        m_Settings.m_dwBadPixelsCount++;
                    }
                else
                    {
                        // Too many pixels
                        //ClearBadPixelsList();
                        return;
                    }
            }

    for (int j = 0; j < m_Settings.m_dwBadPixelsCount; j++)
        {
            // Upper Left Corner
            int x = m_BadPixels[j]->tpCoords.x - 1;
            int y = m_BadPixels[j]->tpCoords.y - 1;
            if ((x >= 0) && (y >= 0))
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Upper
            x = m_BadPixels[j]->tpCoords.x;
            y = m_BadPixels[j]->tpCoords.y - 1;
            if ( y >= 0 )
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Upper Right Corner
            x = m_BadPixels[j]->tpCoords.x + 1;
            y = m_BadPixels[j]->tpCoords.y - 1;
            if ((x < m_Settings.m_dwDataSizeX) && (y >= 0))
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Left
            x = m_BadPixels[j]->tpCoords.x - 1;
            y = m_BadPixels[j]->tpCoords.y;
            if (x >= 0)
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Right
            x = m_BadPixels[j]->tpCoords.x + 1;
            y = m_BadPixels[j]->tpCoords.y;
            if (x < m_Settings.m_dwDataSizeX)
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Lower Left Corner
            x = m_BadPixels[j]->tpCoords.x - 1;
            y = m_BadPixels[j]->tpCoords.y + 1;
            if ((x >= 0) && (y < m_Settings.m_dwDataSizeY))
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Lower
            x = m_BadPixels[j]->tpCoords.x;
            y = m_BadPixels[j]->tpCoords.y + 1;
            if (y < m_Settings.m_dwDataSizeY)
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }

            // Lower Right Corner
            x = m_BadPixels[j]->tpCoords.x + 1;
            y = m_BadPixels[j]->tpCoords.y + 1;
            if ((x < m_Settings.m_dwDataSizeX) && (y < m_Settings.m_dwDataSizeY))
                {
                    if (!CheckBadPixelsList(x, y))
                        {
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.x = x;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].tpCoords.y = y;
                            m_BadPixels[j]->NeigboursList[m_BadPixels[j]->dwNeigbourCount].dwIndexs = y * m_Settings.m_dwDataSizeX + x;
                            m_BadPixels[j]->dwNeigbourCount++;
                        }
                }
        }
}

//-----------------------------------------------------------------------------
// Name: CorrectBadPixels()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::CorrectBadPixels()
{
    UINT16 * pPointer1		= (UINT16 *)m_Settings.m_pBuffer;		// Указатель на пришедший буфер

    for (int i = 0; i < m_Settings.m_dwBadPixelsCount; i++)
        {
            DWORD dwData = 0;
            for (int j = 0; j < m_BadPixels[i]->dwNeigbourCount; j++)
                {
                    dwData += pPointer1[m_BadPixels[i]->NeigboursList[j].dwIndexs];
                }
            if (m_BadPixels[i]->dwNeigbourCount > 0)
                {
                    dwData /= m_BadPixels[i]->dwNeigbourCount;

                    pPointer1[m_BadPixels[i]->dwIndexs] = dwData;
                }                    
        }
}

//-----------------------------------------------------------------------------
// Name: VisualizeBadPixels()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::VisualizeBadPixels()
{
	DDRGBA * pBmpStr1 = (DDRGBA *)m_bitmap.bmBits;  // Указатель на первую строку для записи в BMP
    for (int i = 0; i < m_Settings.m_dwBadPixelsCount; i++)
        {
            UINT16 y = m_BadPixels[i]->tpCoords.y;
            UINT16 x = m_BadPixels[i]->tpCoords.x;
            pBmpStr1[y * m_bitmap.bmWidthBytes / 4 + x].red     = 0;
            pBmpStr1[y * m_bitmap.bmWidthBytes / 4 + x].green   = 0;
            pBmpStr1[y * m_bitmap.bmWidthBytes / 4 + x].blue    = 255;
            pBmpStr1[y * m_bitmap.bmWidthBytes / 4 + x].alpha   = 255;
        }
}

//-----------------------------------------------------------------------------
// Name: ShowNavigationParams()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ShowNavigationParams()
{
	HDC hDC;
	HGDIOBJ hbmOld = NULL;
    HPEN	PenData = NULL;
    HPEN	OldPen = NULL;

	if (m_hbmGistogramm == NULL)
    	return;

	// Clear last bmp
	ZeroMemory(m_bitmapGistogramm.bmBits, m_bitmapGistogramm.bmWidthBytes * m_bitmapGistogramm.bmHeight);

	hDC = CreateCompatibleDC( NULL );
	hbmOld = SelectObject( hDC, m_hbmGistogramm );

	// Create Pen for drawing
	PenData = CreatePen(PS_SOLID, 1, RGB(255, 255, 255));
	// Set Pen and save old pen object
	OldPen = SelectObject(hDC, PenData);
   	// Set initial position
    MoveToEx(hDC, 0, m_bitmapGistogramm.bmHeight, NULL);

    // Настройка шрифта отображения
    SetBkColor(hDC, RGB(0, 255, 0));
    SetBkMode(hDC, TRANSPARENT);
    SetTextColor(hDC, RGB(255, 255, 255));

    UINT8 ucOffset = 10;

    char* cp;
    AnsiString Astr;

    switch ( m_Settings.m_Nav )
    {
        // Матрица МГД
        case 0:

        // Send some text out into the world
        Astr = "MGD[0][0] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[0][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[0][1] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[0][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[0][2] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[0][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[1][0] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[1][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[1][1] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[1][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[1][2] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[1][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[2][0] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[2][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[2][1] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[2][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "MGD[2][2] : " + FloatToStrF(m_Settings.m_navigationIN.MGD_Matrix.data.m[2][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        break;

        // Матрица ОАР
        case 1:

        // Send some text out into the world
        Astr = "ОАР[0][0] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[0][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[0][1] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[0][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[0][2] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[0][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[1][0] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[1][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[1][1] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[1][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[1][2] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[1][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[2][0] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[2][0], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[2][1] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[2][1], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "ОАР[2][2] : " + FloatToStrF(m_Settings.m_navigationIN.OAR_Matrix.data.m[2][2], ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        break;


        // Навигационные данные
        case 2:

        Astr = "Широта : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig3, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Долгота : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig4, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Высота : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig5, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Курс : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig6, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Крен : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig7, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Тангаж : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig8, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Сев. сост. скор. : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig9, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Вост. сост. скор. : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig10, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        Astr = "Верт. сост. скор. : " + FloatToStrF(m_Settings.m_navigationIN.uiNavig11, ffFixed, 6, 4 );
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        break;
    }

	SelectObject( hDC, OldPen);
    SelectObject( hDC, hbmOld );
    DeleteObject( PenData );
    DeleteDC( hDC );
}

//-----------------------------------------------------------------------------
// Name: ShowStatus()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ShowStatus()
{
	HDC hDC;
	HGDIOBJ hbmOld = NULL;
    HPEN	PenData = NULL;
    HPEN	OldPen = NULL;

	if (m_hbmGistogramm == NULL)
    	return;

	// Clear last bmp
	ZeroMemory(m_bitmapGistogramm.bmBits, m_bitmapGistogramm.bmWidthBytes * m_bitmapGistogramm.bmHeight);

	hDC = CreateCompatibleDC( NULL );
	hbmOld = SelectObject( hDC, m_hbmGistogramm );

	// Create Pen for drawing
	PenData = CreatePen(PS_SOLID, 1, RGB(255, 255, 255));
	// Set Pen and save old pen object
	OldPen = SelectObject(hDC, PenData);
   	// Set initial position
    MoveToEx(hDC, 0, m_bitmapGistogramm.bmHeight, NULL);

    // Настройка шрифта отображения
    SetBkColor(hDC, RGB(0, 255, 0));
    SetBkMode(hDC, TRANSPARENT);
    SetTextColor(hDC, RGB(255, 255, 255));

    UINT8 ucOffset = 10;

    char* cp;
    AnsiString Astr;

    switch ( m_Settings.m_CurStat )
    {
        // Общий статус
        case 0:
        // Send some text out into the world
        Astr = "ID посл. ком.: 0x" + IntToHex((__int64)m_Settings.m_OARStatus.uiLastCommandID, 8);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Причина отправки статуса
        //------------------------------------------------------------------------
        Astr = "Причина отправки статуса:";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        switch (m_Settings.m_OARStatus.uiByte1 & 7)
        {
            case 0:
                Astr = "по запросу";
            break;

            case 1:
                Astr = "по срабатыванию таймера";
            break;

            case 2:
                Astr = "изменилось состояния";
            break;

            default:
                Astr = "хер его знает";
            break;
        }

        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Общий статус устройства
        //------------------------------------------------------------------------
        if ( m_Settings.m_OARStatus.uiByte1 & 0x10 )
            Astr = "Статус устр.: неисправно";
        else
            Astr = "Статус устр.: исправно";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Статус датчика
        //------------------------------------------------------------------------
        if ( m_Settings.m_OARStatus.uiByte1 & 0x80 )
            Astr = "Статус датч.: есть ошибки";
        else
            Astr = "Статус датч.: в норме";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Статус обработки команды
        //------------------------------------------------------------------------
        Astr = "Статус обработки команды:";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        switch (m_Settings.m_OARStatus.uiCommStatus)
        {
            case 0:
                Astr = "Команда выполнена успешно";
            break;

            case 1:
                Astr = "Недопуст. параметры команды";
            break;

            case 2:
                Astr = "Недопустимая команда";
            break;

            case 3:
                Astr = "Аппаратная ошибка";
            break;

            default:
                Astr = "хер его знает";
            break;
        }
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Экспозиция
        //------------------------------------------------------------------------
        Astr = "Экспозиция: " + IntToStr(m_Settings.m_OARStatus.uiExposition);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;


        break;

        case 1:

        //------------------------------------------------------------------------
        // Текущая настройка матрицы
        //------------------------------------------------------------------------
        // Send some text out into the world
        if ( m_Settings.m_OARStatus.uiByte2 & 0x80)
            Astr = "Автоэкс.: вкл.";
        else
            Astr = "Автоэкс.: откл.";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        if ( m_Settings.m_OARStatus.uiByte2 & 0x40)
            Astr = "Замена БП: вкл.";
        else
            Astr = "Замена БП: откл.";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        if ( m_Settings.m_OARStatus.uiByte2 & 0x8)
            Astr = "Шторка: закрыта";
        else
            Astr = "Шторка: открыта";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        if ( m_Settings.m_OARStatus.uiByte2 & 0x4)
            Astr = "КГШ: вкл.";
        else
            Astr = "КГШ: отл.";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        if ( m_Settings.m_OARStatus.uiByte2 & 0x2)
            Astr = "Калибр. КГШ: вкл.";
        else
            Astr = "Калибр. КГШ: отл.";
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        break;


        case 2:
        //------------------------------------------------------------------------
        // Данные с температурного датчика 1
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 1: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor1);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 2
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 2: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor2);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 3
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 2: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor3);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 4
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 2: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor4);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 5
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 5: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor5);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 6
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 6: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor6);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 7
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 7: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor7);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        //------------------------------------------------------------------------
        // Данные с температурного датчика 8
        //------------------------------------------------------------------------
        Astr = "Темп. датчик 8: " + IntToStr(m_Settings.m_OARStatus.uiTempSensor8);
        cp = new char[ Astr.Length() + 1 ];
        strcpy( cp, Astr.c_str() );
        TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
        delete[] cp;
        ucOffset += 20;

        break;

    }

	SelectObject( hDC, OldPen);
    SelectObject( hDC, hbmOld );
    DeleteObject( PenData );
    DeleteDC( hDC );
}

//-----------------------------------------------------------------------------
// Name: ShowCaptureParams()
// Desc:
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::ShowCaptureParams()
{
	HDC hDC;
	HGDIOBJ hbmOld = NULL;
    HPEN	PenData = NULL;
    HPEN	OldPen = NULL;

	if (m_hbmGistogramm == NULL)
    	return;

	// Clear last bmp
	ZeroMemory(m_bitmapGistogramm.bmBits, m_bitmapGistogramm.bmWidthBytes * m_bitmapGistogramm.bmHeight);

	hDC = CreateCompatibleDC( NULL );
	hbmOld = SelectObject( hDC, m_hbmGistogramm );

	// Create Pen for drawing
	PenData = CreatePen(PS_SOLID, 1, RGB(255, 255, 255));
	// Set Pen and save old pen object
	OldPen = SelectObject(hDC, PenData);
   	// Set initial position
    MoveToEx(hDC, 0, m_bitmapGistogramm.bmHeight, NULL);

    // Настройка шрифта отображения
    SetBkColor(hDC, RGB(0, 255, 0));
    SetBkMode(hDC, TRANSPARENT);
    SetTextColor(hDC, RGB(255, 255, 255));

    UINT8 ucOffset = 10;

    char* cp;
    AnsiString Astr;

    // Статус записи
    if ( m_Settings.m_bCaptureOn )
        {
            Astr = "Запись данных : включено";
        }
    else
        {
            Astr = "Запись данных : выключено";
        }
    cp = new char[ Astr.Length() + 1 ];
    strcpy( cp, Astr.c_str() );
    TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
    delete[] cp;
    ucOffset += 20;

    Astr = "Кол-во зап. кадров " + IntToStr(m_Settings.m_iFrameCounter);
    cp = new char[ Astr.Length() + 1 ];
    strcpy( cp, Astr.c_str() );
    TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
    delete[] cp;
    ucOffset += 20;

    if ( m_bError )
        Astr = "Ошибка при выборе кодека";
    else
        Astr = "Кодек подключен";
    cp = new char[ Astr.Length() + 1 ];
    strcpy( cp, Astr.c_str() );
    TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
    delete[] cp;
    ucOffset += 20;

    // Статус режима прямого отображения
    if ( m_Settings.m_bDirectDraw )
        Astr = "Режим Direct: включен";
    else
        Astr = "Режим Direct: отключен";
    cp = new char[ Astr.Length() + 1 ];
    strcpy( cp, Astr.c_str() );
    TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
    delete[] cp;
    ucOffset += 20;

    Astr = "HR : 0x" + IntToHex( (__int64)hr_st, 8 );
    cp = new char[ Astr.Length() + 1 ];
    strcpy( cp, Astr.c_str() );
    TextOut(hDC, 10, ucOffset, cp,  _tcslen(cp));
    delete[] cp;
    ucOffset += 20;

	SelectObject( hDC, OldPen);
    SelectObject( hDC, hbmOld );
    DeleteObject( PenData );
    DeleteDC( hDC );
}

//-----------------------------------------------------------------------------
// Name: CaptureVideoToAVI()
// Desc: Запись данных потока в файл AVI
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::CaptureVideoToAVI()
{
    if ( m_Settings.m_bCaptureOn )
        {
            if ( m_Settings.m_iFrameCounter < MAX_CAPTURE )
                {
                    m_Settings.m_iSleepCounter++;
                    if ( m_Settings.m_iSleepCounter >= FRAME_BETWEEN_CAPTURE )
                        {
                            if (!m_Settings.m_bDirectDraw)
                                // Используется изображение, которое выводится на экран
                                AddAviFrame( m_avi, m_pD3DBitmap->GetBitmap() );
                            else
                                // В режиме прямой записи используется специально построенное изображение
                                AddAviFrame( m_avi, m_hbmBitmap );

                            m_Settings.m_iSleepCounter = 0;
                            m_Settings.m_iFrameCounter++;
                        }
                }
            else
                {
                    // Cloase AVI capture file
                    CloseAvi( m_avi );
                    m_avi = NULL;

                    m_Settings.m_bCaptureOn = false;
                }
        }
}


//-----------------------------------------------------------------------------
// Name: SetCaptureVideo()
// Desc: Включение/отключения записи в файл
//-----------------------------------------------------------------------------
void __fastcall TCamCAMOAR::SetCaptureVideo()
{
    if ( m_bCapturing == m_Settings.m_bCaptureOn )
        return;

    // Остановить запись видео
    if ( !m_Settings.m_bCaptureOn )
        {
            if ( m_avi )
                {
                    CloseAvi( m_avi );
                    m_avi = NULL;
                }

            m_bCapturing = false;

            return;
        }


    m_bCapturing = false;
    m_bError     = false;
    // Обнулить счетчики перед новой записью
    m_Settings.m_iSleepCounter = 0;
    m_Settings.m_iFrameCounter = 0;
    // Создать AVI
    m_avi = CreateAvi( "test.avi", FRAME_RATE );
    if ( m_avi == NULL )
        return;

    // Требуется сжатое видео
    if ( m_Settings.m_bNeedCompress )
        {
            // Set up compression just before the first frame
            HBITMAP hbm = 0;
            if ( m_Settings.m_bDirectDraw )
                hbm = m_hbmBitmap;
            else
                hbm = m_pD3DBitmap->GetBitmap();

            // Нужно ли отображать диалог
            bool bShowDialog = m_bFirstTime;
            if ( bShowDialog )
                {
                    ZeroMemory( &m_opts, sizeof( m_opts ) );
                    m_opts.fccHandler = mmioFOURCC('d','i','v','x');
                }

            // Настроить параметры сжатия
            HRESULT hr = SetAviVideoCompression( m_avi, hbm, &m_opts, bShowDialog, 0);
            if ( hr != AVIERR_OK )
                {
                    // Произошла ошибка при попытке выбрать кодек
                    m_bError     = true;
                    m_bFirstTime = true;
                    m_bCapturing = false;
                    m_Settings.m_bCaptureOn = false;

                    // Закрыть файл
                    if ( m_avi )
                        {
                            CloseAvi( m_avi );
                            m_avi = NULL;
                        }

                    return;
                }

            m_bFirstTime = false;
        }

    m_bCapturing = true;
}

