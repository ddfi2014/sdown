Imports System.IO
Imports System.Threading.Tasks
Imports System.Timers
Imports Microsoft.Win32

Public Class ServerDownClientControl
#Region "Declarations"
    Private proxy As New GetLogClient()
    Private clientView As MainWindow
    Private serverLog As String()
    Private fullLog As New List(Of String)()
    Private timer As Timers.Timer
    Private Const interval As Double = 1000 * 1 * 15
    Private Const stringSaveDialogFilter As String = "Textdateien (*.txt, *.log)|*.txt;*.log|Alle Dateien (*.*)|*.*"
    Private Const stringLogSaved As String = "Log saved as: "
    Private Const stringLogNotSaved As String = "Log could not be saved."
    Private Const stringServerNotConnected As String = "Error: Server could not be reached."
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
        clientView.listBoxLog.Items.Clear()
        clientView.StatusMessage = ""
    End Sub

    Private Sub UpdateListBoxPosition(listBox As ListBox)
        If listBox IsNot Nothing Then
            Dim _border = TryCast(VisualTreeHelper.GetChild(listBox, 0), Border)
            Dim _scrollViewer = TryCast(VisualTreeHelper.GetChild(_border, 0), ScrollViewer)
            _scrollViewer.ScrollToBottom()
        End If
    End Sub

    ''' <summary>
    ''' Obtains the server's log.
    ''' </summary>
    Public Sub GetTextLog()
        Try
            serverLog = proxy.GetServerLog()
            serverLog.ToList().ForEach(Sub(x) fullLog.Add(x))
            clientView.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub() UpdateList())
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            clientView.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub() clientView.AddLogItem(Date.UtcNow.ToString() & " " & stringServerNotConnected))
            clientView.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub() UpdateListBoxPosition(clientView.StatusLogList))
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
            'serverLog.ToList().ForEach(Sub(line) sw.WriteLine(line))
            fullLog.ForEach(Sub(line) sw.WriteLine(line))
            sw.Flush()
            clientView.StatusMessage = stringLogSaved & filePath & "."
        Catch ex As Exception
            clientView.StatusMessage = stringLogNotSaved
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
        'clientView.statusBarItemMessage.Content = "Log saved as: " & defaultSavePath & "."
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
                'clientView.statusBarItemMessage.Content = stringLogSaved & fileDialog.FileName & "."
                clientView.StatusMessage = stringLogSaved & fileDialog.FileName & "."
            End If
        Catch ex As Exception
            'clientView.statusBarItemMessage.Content = stringLogNotSaved
            clientView.StatusMessage = stringLogNotSaved
        End Try
    End Sub

    ''' <summary>
    ''' Saves a logfile under the specified path.
    ''' </summary>
    ''' <param name="filePath">The path that refers to the location of the file and its name.
    ''' <para>Example: <c>C:\...\filename.txt</c></para></param>
    Public Sub SaveAs(filePath As String)
        WriteFile(filePath)
        'clientView.statusBarItemMessage.Content = stringLogSaved & filePath & "."
        clientView.StatusMessage = stringLogSaved & filePath & "."
    End Sub

    Private Function GetDefaultSavePath() As String
        Dim defaultSavePath As String = Path.GetFullPath("log_" + GetCustomDateTimeString() + ".log")
        Return defaultSavePath
    End Function

    ''' <summary>
    ''' Creates the current date as a string.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetCustomDateTimeString() As String
        Return Date.UtcNow.ToLocalTime().ToString("yyyyMMdd_HHmmss")
    End Function
#End Region
#Region "Timer"
    ''' <summary>
    ''' Starts the timer that the server's log at the set interval.
    ''' </summary>
    Private Sub StartTimer()
        timer = New Timers.Timer(interval)
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
        UpdateListBoxPosition(clientView.listBoxLog)
        'serverLog.ToList().ForEach(Sub(x) clientView.listBoxLog.Items.Add(x))
        serverLog.ToList().ForEach(Sub(x) clientView.AddLogItem(x))
    End Sub
#End Region
#Region "Events"
    Private Sub OnTimedEvent(sender As Object, e As ElapsedEventArgs)
        GetTextLog()
    End Sub
#End Region
End Class