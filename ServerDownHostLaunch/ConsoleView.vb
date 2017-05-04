Imports System.Windows.Threading
Imports ServerDownClassLibrary
Public Class ConsoleView
    Implements IView
#Region "Declarations"
    Private args As String()
    Private result As String
    Private control As ServerDownHost.ServerDownHostControl
    Private _dispatcher As Dispatcher = Dispatcher.CurrentDispatcher
    Private logList As New List(Of String)()
    Private viewList As IViewList
    Public Property StatusMessage As String
#End Region
#Region "Instance"
    Private Sub New()

    End Sub

    Friend Sub New(args As String())
        Me.New()
        Me.args = args
    End Sub
#End Region
#Region "Display"
    Friend Sub Start()
        viewList = New ViewList(logList)
        control = ServerDownHost.ServerDownHostControl.GetInstance()
        ServerDownHost.ServerDownHostControl.AddView(view:=Me)
        ShowOptions()
    End Sub

    Private Sub ShowOptions()
        Console.WriteLine("What do you want to do?")
        Console.WriteLine("* Log")
        Console.WriteLine("* Host")
        Console.WriteLine("* StopLog")
        Console.WriteLine("* StopHost")
        Console.WriteLine("* Exit")
        Console.WriteLine()
        Reply()
    End Sub

    Private Sub Reply(Optional withNewLine As String = "")
        Console.Write(withNewLine & "ServerDown>")
        result = Console.ReadLine()
        Select Case result.ToLower()
            Case "Log".ToLower()
                StartLog()
            Case "Host".ToLower()
                StartHost()
            Case "StopLog".ToLower()
                StopLog()
            Case "StopHost".ToLower()
                StopHost()
            Case "Exit".ToLower()
                Close()
            Case "Help".ToLower()
                ShowOptions()
            Case ""
                Reply()
            Case Else
                Console.WriteLine("Invalid Option.")
                Reply(vbNewLine)
        End Select
    End Sub

    Private Sub Close()
        control.SetLogOff()
        control.StopHost()
    End Sub

    Private Sub StartLog()
        Console.WriteLine("Logging start.")
        control.SetLogOn()
        control.StartSaveTimer()
        Reply(vbNewLine)
    End Sub

    Private Sub StartHost()
        Console.WriteLine("Hosting start.")
        control.StartHost()
        Reply(vbNewLine)
    End Sub

    Private Sub StopLog()
        Console.WriteLine("Logging stop.")
        control.SetLogOff()
        Reply(vbNewLine)
    End Sub

    Private Sub StopHost()
        Console.WriteLine("Hosting stop.")
        control.StopHost()
        Reply(vbNewLine)
    End Sub
#End Region
#Region "Implementations"
    Public Sub AddLogItem(item As String) Implements IView.AddLogItem
        logList.Add(item)
        Console.WriteLine(item)
    End Sub

    Public Sub ClearList() Implements IView.ClearList
        Console.Clear()
    End Sub

    Public Sub ShowErrorMessage(message As String, Optional errorlevel As EErrorLevel = EErrorLevel.None) Implements IView.ShowErrorMessage
        Console.WriteLine(message)
    End Sub

    Public Sub Dispatch(priority As DispatcherPriority, method As [Delegate]) Implements IView.Dispatch
        Console.CursorLeft = 0
        method.DynamicInvoke()
        Console.Write("ServerDown>")
    End Sub

    Public Function GetStatusMessage() As String Implements IView.GetStatusMessage
        Return StatusMessage
    End Function

    Public Sub SetStatusMessage(message As String) Implements IView.SetStatusMessage
        StatusMessage = message
    End Sub

    Public Function GetStatusLogList() As IViewList Implements IView.GetStatusLogList
        Return viewList
    End Function
#End Region
End Class
