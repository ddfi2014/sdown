Public Class Server
#Region "Declarations"
#Region "Attributes"
    Private Shared maxId As Integer = 0
    Private hostId As Integer
    Private hostName As String
    Private detailedHostName As String
    Private hostIPString As String
    Private hostType As EHostType
    Private hostState As String
    Private hostTime As Date
    Public Shared Servers As New List(Of Server)()
#End Region
#Region "Constants"
    Private Const intInvalidHostValue As Integer = -1
    Private Const stringServerIsRunning As String = "The server is running."
    Private Const stringServerCannotBeReached As String = "The server cannot be reached."
    Private Const stringProcessIsRunning As String = "The process is running."
    Private Const stringProcessIsNotRunning As String = "The process is not running."
    Private Const stringServiceIsRunning As String = "The service is running."
    Private Const stringServiceIsNotRunning As String = "The service is not running."
#End Region
#Region "Properties"
    Public ReadOnly Property GetHostName() As String
        Get
            Return hostName
        End Get
    End Property

    Public ReadOnly Property GetDetailedHostName() As String
        Get
            Return detailedHostName
        End Get
    End Property

    Public ReadOnly Property GetHostIPString() As String
        Get
            Return hostIPString
        End Get
    End Property

    Public ReadOnly Property GetHostType() As EHostType
        Get
            Return hostType
        End Get
    End Property

    Public ReadOnly Property GetHostState() As String
        Get
            Return hostState
        End Get
    End Property

    Public ReadOnly Property GetHostTime() As Date
        Get
            Return hostTime
        End Get
    End Property

    Public ReadOnly Property GetHostTimeString() As String
        Get
            Return hostTime.ToLocalTime().ToString()
        End Get
    End Property
#End Region
#End Region
#Region "Instance"
    Public Sub New(hostName As String, detailedHostName As String, hostIP As String, hostType As String)
        hostId = maxId
        maxId += 1
        Me.hostName = hostName
        Me.detailedHostName = detailedHostName
        Me.hostIPString = hostIP
        Me.hostType = ConvertStringToHostType(hostType)
        hostTime = Date.UtcNow.ToLocalTime()
    End Sub
#End Region
#Region "Conversions"
    ''' <summary>
    ''' Converts a <c>HostType</c> string into an <c>EHostType</c>.
    ''' </summary>
    ''' <param name="hostType"></param>
    ''' <returns></returns>
    Private Function ConvertStringToHostType(hostType As String) As EHostType
        Dim type As EHostType
        Select Case hostType
            Case "Server"
                type = EHostType.Server
            Case "Process"
                type = EHostType.Process
            Case "Service"
                type = EHostType.Service
            Case Else
                type = EHostType.Invalid
        End Select
        Return type
    End Function
#End Region
#Region "Evaluation"
    ''' <summary>
    ''' Evaluates the validity of hosts and their accessability.
    ''' </summary>
    Public Shared Sub EvaluateHosts()
        Servers.ForEach(Sub(host) host.EvaluateHostType())
        Servers.RemoveAll(Function(x) -1 = x.hostId)
    End Sub

    ''' <summary>
    ''' Evaluates the host's <c>HostType</c> and prepares invalid hosts for removal.
    ''' </summary>
    Private Sub EvaluateHostType()
        Try
            Select Case hostType
                Case EHostType.Server
                    EvaluateServer()
                Case EHostType.Process
                    EvaluateProcess()
                Case EHostType.Service
                    EvaluateService()
                Case Else
                    Throw New ArgumentException("Error: The HostType is invalid.")
            End Select
        Catch ex As ArgumentException
            hostId = intInvalidHostValue
        End Try
    End Sub

    ''' <summary>
    ''' Evaluates, if a server is running and sets the <c>HostState</c> appropiately.
    ''' </summary>
    Private Sub EvaluateServer()
        hostState = If(TestServer(), stringServerIsRunning, stringServerCannotBeReached)
    End Sub

    ''' <summary>
    ''' Evaluates, if a process is running and sets the <c>HostState</c> appropiately.
    ''' </summary>
    Private Sub EvaluateProcess()
        If TestServer() Then
            hostState = If(TestProcess(), stringProcessIsRunning, stringProcessIsNotRunning)
        Else
            hostState = stringServerCannotBeReached
        End If
    End Sub

    ''' <summary>
    ''' Evaluates, if a service is running and sets the <c>HostState</c> appropriately.
    ''' </summary>
    Private Sub EvaluateService()
        If TestServer() Then
            hostState = If(TestService(), stringServiceIsRunning, stringServiceIsNotRunning)
        Else
            hostState = stringServerCannotBeReached
        End If
    End Sub
#End Region
#Region "Tests"
    ''' <summary>
    ''' Tests, whether a server is accessable or not and returns the result.
    ''' </summary>
    ''' <returns></returns>
    Private Function TestServer() As Boolean
        Dim result As Boolean = False
        Try
            Dim p As New Net.NetworkInformation.Ping()
            result = If(p.Send(hostIPString, 100).Status = Net.NetworkInformation.IPStatus.Success, True, False)
        Catch ex As Net.NetworkInformation.PingException
            result = False
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Tests, whether a process runs or not and returns the result.
    ''' </summary>
    ''' <returns></returns>
    Private Function TestProcess() As Boolean
        Dim result As Boolean = False
        Try
            result = Process.GetProcesses(hostIPString).ToList().Exists(Function(x) x.ProcessName.ToLower() = hostName.ToLower())
        Catch ex As InvalidOperationException
            If hostIPString.Equals("127.0.0.1") Or hostIPString.Equals("localhost") Then
                Try
                    result = Process.GetProcesses().ToList().Exists(Function(x) x.ProcessName.ToLower() = hostName.ToLower())
                Catch ex1 As Exception
                    result = False
                End Try
            End If
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Tests, whether a service runs or not and returns the result.
    ''' </summary>
    ''' <returns></returns>
    Private Function TestService() As Boolean
        Dim result As Boolean = False
        Try
            result = ServiceProcess.ServiceController.GetServices(hostIPString).Select(Function(x) x.ServiceName.ToLower() = hostName.ToLower() And x.Status = ServiceProcess.ServiceControllerStatus.Running).Any()
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            result = False
        End Try
        Return result
    End Function
#End Region
#Region "Overrides"
    Public Overrides Function ToString() As String
        Return hostTime.ToLocalTime().ToString() & ": " & hostType.ToString() & " " & hostName & "(" & hostIPString & ")" & ": " & hostState
    End Function
#End Region
End Class
