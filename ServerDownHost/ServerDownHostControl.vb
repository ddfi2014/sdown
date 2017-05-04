Imports System.IO
Imports System.ServiceModel
Imports System.Timers
Imports ServerDownClassLibrary

Public Class ServerDownHostControl
#Region "Declarations"
#Region "Fields"
    Private sh As ServiceHost
    Private Shared instance As ServerDownHostControl = Nothing
    Private Shared views As List(Of IView) = Nothing
    Private isLogEnabled As Boolean = False
    Private isSaveEnabled As Boolean = False
    Private logText As New List(Of String)
    Private logTimer As Timer = Nothing
    Private saveTimer As Timer = Nothing
    Private defaultLogFileSavePath As String = Path.Combine(Directory.GetCurrentDirectory(), "logfile.log")
    Private logFileText As New List(Of String)
    Private statusMessage As String = ""
#End Region
#Region "Constants"
    Private Const logInterval As Double = 1000 * 1 * 5 'Critical Error after a while with low intervals; 10 seconds minimum!
    Private Const saveInterval As Double = 1000 * 1 * 15
    Private Const stringLogFileSaveFailure As String = "The logfile could not be saved."
    Private Const stringServiceReady As String = "Service ready."
    Private Const stringServiceStopped As String = "Service stopped."
    Private Const stringServiceStopFailure As String = "Error: Could not stop service."
    Private Const stringNewReportText As String = "New Report: "
#End Region
#End Region
#Region "Singleton-Pattern"
    Public Shared Function GetInstance() As ServerDownHostControl
        If instance Is Nothing Then
            instance = New ServerDownHostControl()
            views = New List(Of IView)()
        End If
        Return instance
    End Function
#End Region
#Region "Control"
    ''' <summary>
    ''' Adds a view to the list of controlled views.
    ''' </summary>
    ''' <param name="view"></param>
    Public Shared Sub AddView(ByRef view As IView)
        GetInstance().InitializeWindow(view)
        views.Add(view)
    End Sub

    ''' <summary>
    ''' Initializes all views.
    ''' </summary>
    Public Sub InitializeWindow(ByRef view As IView)
        view.ClearList()
        UpdateView(view)
        Try
            Dim mw As MainWindow = view
            AddHandler mw.buttonStart.Click, Sub() InitializeHost()
            AddHandler mw.buttonStop.Click, Sub() CloseHost()
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Updates the position of the IView's ListBox to show the most recent entry.
    ''' </summary>
    ''' <param name="view"></param>
    Private Sub UpdateListBoxPosition(view As IView)
        Try
            Dim listBox As ListBox = view.GetStatusLogList().GetList()
            If listBox IsNot Nothing Then
                Dim _border = TryCast(VisualTreeHelper.GetChild(listBox, 0), Border)
                Dim _scrollViewer = TryCast(VisualTreeHelper.GetChild(_border, 0), ScrollViewer)
                _scrollViewer.ScrollToBottom()
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Starts the hosting.
    ''' </summary>
    Public Sub InitializeHost()
        StartLogTimer()
        StartSaveTimer()
        StartHost()
    End Sub

    Public Sub StartHost()
        If sh Is Nothing Or sh?.State = CommunicationState.Closed Then
            sh = New ServiceHost(serviceType:=GetType(GetLog))
            sh.Open()
            views.ForEach(Sub(view) UpdateView(view, stringServiceReady))
        End If
    End Sub

    ''' <summary>
    ''' Stops the hosting.
    ''' </summary>
    Public Sub CloseHost()
        StopLogTimer()
        StopSaveTimer()
        StopHost()
    End Sub

    Public Sub StopHost()
        Try
            sh.Close()
            views.ForEach(Sub(view) UpdateView(view, stringServiceStopped))
        Catch ex As Exception
            views.ForEach(Sub(view) UpdateView(view, stringServiceStopFailure))
        End Try
    End Sub

    ''' <summary>
    ''' Returns the <c>LogText</c>.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLogText() As String()
        Return logText.ToArray()
    End Function
#End Region
#Region "Logging"
#Region "ToggleLogState"
    ''' <summary>
    ''' Turns the logging process on.
    ''' </summary>
    Public Sub SetLogOn()
        If isLogEnabled = False Then
            Try
                StartLogTimer()
                isLogEnabled = True
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                isLogEnabled = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Turns the logging process off.
    ''' </summary>
    Public Sub SetLogOff()
        If isLogEnabled Then
            Try
                StopLogTimer()
                isLogEnabled = False
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                isLogEnabled = True
            End Try
        End If
    End Sub
#End Region
#Region "Update"
    ''' <summary>
    ''' Tests, if the specified servers are running and updates the views.
    ''' </summary>
    Private Sub TestServers()
        logText = ServerTest.GetServerStateStrings()
        logFileText.AddRange(logText)
        views.ForEach(Sub(view) view.Dispatch(Threading.DispatcherPriority.Send, Sub() UpdateListBox(view)))
    End Sub

    Private Sub UpdateView(ByRef view As IView)
        view.SetStatusMessage(statusMessage)
    End Sub

    Private Sub UpdateView(ByRef view As IView, ByVal message As String)
        view.SetStatusMessage(message)
    End Sub
#End Region
#Region "SaveLog"
    ''' <summary>
    ''' Saves the log as a file at the default location.
    ''' </summary>
    Private Sub SaveLogFile()
        Dim sw As StreamWriter = StreamWriter.Null
        Try
            sw = New StreamWriter(path:=defaultLogFileSavePath, append:=True, encoding:=Text.Encoding.UTF8)
            logFileText.ForEach(Sub(line) sw.WriteLine(line))
            logFileText.Clear()
            sw.Flush()
        Catch ex As Exception
            statusMessage = stringLogFileSaveFailure
            views.ForEach(Sub(view) view.Dispatch(Threading.DispatcherPriority.Background, Sub()
                                                                                            UpdateView(view)
                                                                                            view.ShowErrorMessage(stringLogFileSaveFailure, EErrorLevel.Warning)
                                                                                           End Sub))
        Finally
            sw.Close()
        End Try
    End Sub
#End Region
#End Region
#Region "Timer"
    ''' <summary>
    ''' Starts the timer, that triggers an event after a specified amount of time has passed.
    ''' </summary>
    Private Sub StartLogTimer()
        If logTimer Is Nothing Or logTimer?.Enabled = False Then
            logTimer = New Timer(logInterval)
            AddHandler logTimer.Elapsed, Sub() TestServers()
            logTimer.AutoReset = True
            logTimer.Start()
        End If
    End Sub

    ''' <summary>
    ''' Initializes and starts the <c>saveTimer</c>.
    ''' </summary>
    Public Sub StartSaveTimer()
        If saveTimer Is Nothing Or saveTimer?.Enabled = False Then
            saveTimer = New Timer(saveInterval)
            AddHandler saveTimer.Elapsed, Sub() SaveLogFile()
            saveTimer.AutoReset = True
            saveTimer.Start()
        End If
    End Sub

    ''' <summary>
    ''' Stops the <c>logTimer</c>.
    ''' </summary>
    Private Sub StopLogTimer()
        If logTimer?.Enabled Then
            logTimer.Stop()
        End If
    End Sub

    ''' <summary>
    ''' Stops the <c>saveTimer</c>.
    ''' </summary>
    Public Sub StopSaveTimer()
        If saveTimer?.Enabled Then
            saveTimer.Stop()
        End If
    End Sub
#End Region
#Region "Delegates"
    ''' <summary>
    ''' Updates the view's listBox.
    ''' </summary>
    ''' <param name="view"></param>
    Private Sub UpdateListBox(view As IView)
        logText.ForEach(Sub(item) view.AddLogItem(item))
        UpdateListBoxPosition(view)
        view.SetStatusMessage(stringNewReportText & Date.UtcNow.ToLocalTime().ToString())
    End Sub
#End Region
End Class