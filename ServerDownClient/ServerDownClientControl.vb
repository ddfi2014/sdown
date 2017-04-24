Imports System.IO
Imports System.Threading.Tasks
Imports System.Timers
Imports Microsoft.Win32

Public Class ServerDownClientControl

#Region "Declarations"
    Private proxy As New GetLogClient()
    Private clientView As MainWindow
    Public serverLog As String()
    Private timer As Timers.Timer
    Private Const interval As Double = 1000 * 15
#End Region

#Region "Instance"
    Public Sub New()
#Region "Fallback"
        'timer = New Timers.Timer(interval)
        'AddHandler timer.Elapsed, AddressOf OnTimedEvent
        'timer.AutoReset = True
        'timer.Start()
#End Region
        StartTimer()
    End Sub

    Public Sub New(clientView As MainWindow)
        Me.New()
#Region "Fallback"
        'timer = New Timers.Timer(interval)
        'AddHandler timer.Elapsed, AddressOf OnTimedEvent
        'timer.AutoReset = True
        'timer.Start()
#End Region
        Me.clientView = clientView
        InitializeWindow()
    End Sub
#End Region

#Region "Functionality"
    Public Sub InitializeWindow()
        'clientView.listBoxLog.Items.Clear()
        clientView.listBoxLog.ItemsSource = serverLog
        clientView.statusBarItemMessage.Content = ""
    End Sub

    ''' <summary>
    ''' Obtains the server's log.
    ''' </summary>
    Public Sub GetTextLog()
        Try
            serverLog = proxy.GetServerLog()
            clientView.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, New Action(Sub() UpdateList()))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try
    End Sub

    ''' <summary>
    ''' Writes a file with the contents of the ListBox to the specified path.
    ''' </summary>
    ''' <param name="filePath"></param>
    Private Sub WriteFile(filePath As String)
        Using sw As New StreamWriter(filePath, False, Text.Encoding.UTF8)
            For Each line In serverLog
                sw.WriteLine(line)
            Next
            sw.Flush()
            sw.Close()
        End Using
    End Sub

    ''' <summary>
    ''' Saves a logfile in the same directory from which the program is being run from.
    ''' </summary>
    Public Sub Save()
        Dim defaultSavePath As String = GetDefaultSavePath()
        WriteFile(defaultSavePath)
        clientView.statusBarItemMessage.Content = "Log saved as: " & defaultSavePath & "."
    End Sub

    ''' <summary>
    ''' Opens a SaveFileDialog that allows the user to select the name and location of a new logfile.
    ''' </summary>
    Public Sub SaveAs()
        Dim fileDialog As New SaveFileDialog()
        fileDialog.InitialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory())
        fileDialog.Filter = "Textdateien (*.txt, *.log)|*.txt;*.log|Alle Dateien (*.*)|*.*"
        fileDialog.AddExtension = True
        Try
            If fileDialog.ShowDialog() Then
                WriteFile(fileDialog.FileName)
                clientView.statusBarItemMessage.Content = "Log saved as: " & fileDialog.FileName & "."
            End If
        Catch ex As Exception
            clientView.statusBarItemMessage.Content = "Error: File could not be saved."
        End Try
    End Sub

    ''' <summary>
    ''' Saves a logfile under the specified path.
    ''' </summary>
    ''' <param name="filePath">The path that refers to the location of the file and its name.
    ''' <para>Example: <c>C:\...\filename.txt</c></para></param>
    Public Sub SaveAs(filePath As String)
        WriteFile(filePath)
        clientView.statusBarItemMessage.Content = "Log saved as: " & filePath & "."
    End Sub

    Private Function GetDefaultSavePath() As String
        Dim defaultSavePath As String = Path.GetFullPath("log_" + GetCustomDateTimeString() + ".txt")
        Return defaultSavePath
    End Function

    ''' <summary>
    ''' Creates the current date as a string.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetCustomDateTimeString() As String
        'Dim dateString As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        Return DateTime.Now.ToString("yyyyMMdd_HHmmss")
    End Function
#End Region

#Region "Timer"
    ''' <summary>
    ''' Starts the timer that the server's log at the set interval.
    ''' </summary>
    Private Sub StartTimer()
        timer = New Timers.Timer(interval)
        AddHandler timer.Elapsed, AddressOf OnTimedEvent
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
        clientView.listBoxLog.ItemsSource = serverLog
    End Sub
#End Region

#Region "Events"
    Private Sub OnTimedEvent(sender As Object, e As ElapsedEventArgs)
        GetTextLog()
    End Sub
#End Region

End Class