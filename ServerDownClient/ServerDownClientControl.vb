Imports System.IO
Imports System.Timers
Imports Microsoft.Win32
Imports ServerDownClassLibrary

Public Class ServerDownClientControl
#Region "Declarations"
    Private proxy As New GetLogClient()
    Private clientView As IView
    Private serverLog As String()
    Private fullLog As New List(Of String)()
    Private timer As Timer
    Private Const interval As Double = 1000 * 1 * 10
    Private Const stringSaveDialogFilter As String = "Textdateien (*.txt, *.log)|*.txt;*.log|Alle Dateien (*.*)|*.*"
    Private Const stringLogSaved As String = "Log saved as: "
    Private Const stringLogNotSaved As String = "Log could not be saved."
    Private Const stringServerNotConnected As String = "Error: Server could not be reached."
    Public ReadOnly Property LocalDateTimeString() As String
        Get
            Return Date.UtcNow.ToLocalTime().ToString()
        End Get
    End Property
    Public ReadOnly Property CustomLocalDateTimeString() As String
        Get
            Return Date.UtcNow.ToLocalTime().ToString("yyyyMMdd_HHmmss")
        End Get
    End Property
#End Region
#Region "Instance"
    Public Sub New()
        StartTimer()
    End Sub

    Public Sub New(clientView As MainWindow)
        Me.New()
        Me.clientView = clientView
        InitializeWindow()
    End Sub
#End Region
#Region "Control"
    Public Sub InitializeWindow()
        clientView.ClearList()
        clientView.SetStatusMessage("")
        Try
            Dim mw As MainWindow = clientView
            AddHandler mw.buttonSaveLog.Click, Sub() Save()
            AddHandler mw.menuItemOptionsSave.Click, Sub() Save()
            AddHandler mw.menuItemOptionsSaveAs.Click, Sub() SaveAs()
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Updates the position of the View's <c>IViewList</c>, if it is a <c>ListBox</c>.
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
    ''' Obtains the server's log.
    ''' </summary>
    Public Sub GetTextLog()
        Try
            serverLog = proxy.GetServerLog()
            serverLog.ToList().ForEach(Sub(x) fullLog.Add(x))
            clientView.Dispatch(Threading.DispatcherPriority.Send, Sub() UpdateList())
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            clientView.Dispatch(Threading.DispatcherPriority.Background, Sub()
                                                                             clientView.AddLogItem(LocalDateTimeString & " " & stringServerNotConnected)
                                                                             UpdateListBoxPosition(clientView)
                                                                         End Sub)
        End Try
    End Sub

    ''' <summary>
    ''' Writes a file with the contents of the ListBox to the specified path.
    ''' </summary>
    ''' <param name="filePath"></param>
    Private Sub WriteFile(filePath As String)
        Dim sw As StreamWriter = StreamWriter.Null
        Try
            sw = New StreamWriter(path:=filePath, append:=False, encoding:=Text.Encoding.UTF8)
            fullLog.ForEach(Sub(line) sw.WriteLine(line))
            sw.Flush()
            clientView.SetStatusMessage(stringLogSaved & filePath & ".")
        Catch ex As Exception
            clientView.SetStatusMessage(stringLogNotSaved)
            MessageBox.Show(stringLogNotSaved, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None)
        Finally
            sw.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Saves a logfile in the same directory from which the program is being run from.
    ''' </summary>
    Public Sub Save()
        Dim defaultSavePath As String = GetDefaultSavePath()
        WriteFile(defaultSavePath)
    End Sub

    ''' <summary>
    ''' Opens a SaveFileDialog that allows the user to select the name and location of a new logfile.
    ''' </summary>
    Public Sub SaveAs()
        Dim fileDialog As New SaveFileDialog()
        fileDialog.InitialDirectory = Directory.GetCurrentDirectory()
        fileDialog.Filter = stringSaveDialogFilter
        fileDialog.AddExtension = True
        Try
            If fileDialog.ShowDialog() Then
                WriteFile(fileDialog.FileName)
                clientView.SetStatusMessage(stringLogSaved & fileDialog.FileName & ".")
            End If
        Catch ex As Exception
            clientView.SetStatusMessage(stringLogNotSaved)
        End Try
    End Sub

    ''' <summary>
    ''' Saves a logfile under the specified path.
    ''' </summary>
    ''' <param name="filePath">The path that refers to the location of the file and its name.
    ''' <para>Example: <c>C:\...\filename.txt</c></para></param>
    Public Sub SaveAs(filePath As String)
        WriteFile(filePath)
        clientView.SetStatusMessage(stringLogSaved & filePath & ".")
    End Sub

    Private Function GetDefaultSavePath() As String
        Dim defaultSavePath As String = Path.GetFullPath("log_" + CustomLocalDateTimeString + ".log")
        Return defaultSavePath
    End Function
#End Region
#Region "Timer"
    ''' <summary>
    ''' Starts the timer that the server's log at the set interval.
    ''' </summary>
    Private Sub StartTimer()
        timer = New Timer(interval)
        AddHandler timer.Elapsed, Sub() GetTextLog()
        timer.AutoReset = True
        timer.Start()
    End Sub

    ''' <summary>
    ''' Stops the timer.
    ''' </summary>
    Public Sub StopTimer()
        timer.Stop()
    End Sub
#End Region
#Region "Delegates"
    Public Sub UpdateList()
        UpdateListBoxPosition(clientView)
        serverLog.ToList().ForEach(Sub(item) clientView.AddLogItem(item))
    End Sub
#End Region
End Class