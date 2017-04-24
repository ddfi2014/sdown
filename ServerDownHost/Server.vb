Imports System.Windows.Threading

Public Class Server

#Region "Declarations"
    Private hostId As Integer
    Private hostName As String
    Private detailedHostName As String
    Private hostIP As String
    Private hostType As EHostType
    Private hostState As String
    Public Shared Servers As New List(Of Server)()
    Private Shared maxId As Integer = 0

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

    Public ReadOnly Property GetHostIP() As String
        Get
            Return hostIP
        End Get
    End Property

    Public ReadOnly Property GetHostType As EHostType
        Get
            Return hostType
        End Get
    End Property

    Public ReadOnly Property GetHostState As String
        Get
            Return hostState
        End Get
    End Property
#End Region

#Region "Instance"
    Public Sub New(hostName As String, detailedHostName As String, hostIP As String, hostType As String)
        hostId = maxId
        maxId += 1
        Me.hostName = hostName
        Me.detailedHostName = detailedHostName
        Me.hostIP = hostIP
        Me.hostType = ConvertStringToHostType(hostType)
    End Sub
#End Region

#Region "Functionality"
    Private Sub PrepareInvalidHostsForRemoval()
        'Servers.Remove(Servers.Find(Function(x) x.hostId = hostId))
        hostId = -1
    End Sub

    Private Shared Sub RemoveInvalidHosts()
        Servers.Remove(Servers.Find(Function(x) -1 = x.hostId))
    End Sub

    Public Shared Sub TestHosts()
        For Each host In Servers
            host.TestHost()
        Next
        RemoveInvalidHosts()
    End Sub

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

    Private Sub TestHost()
        Try
            Select Case hostType
                Case EHostType.Server
                    TestServer()
                Case EHostType.Process
                    TestProcess()
                Case EHostType.Service
                    TestService()
                Case Else
                    Throw New ArgumentException("Error: The HostType is invalid.")
            End Select
        Catch ex As ArgumentException
            PrepareInvalidHostsForRemoval()
        End Try
    End Sub

    Private Sub TestServer()
        If My.Computer.Network.Ping(hostIP) Then
            hostState = "Connection successful."
        Else
            hostState = "Connection unsuccessful."
        End If
    End Sub

    Private Sub TestProcess()
        hostState = "Feature not yet implemented."
        Throw New NotImplementedException("Error: TestProcess()")
    End Sub

    Private Sub TestService()
        hostState = "Feature not yet implemented."
        Throw New NotImplementedException("Error: TestService()")
    End Sub
#End Region

#Region "Overrides"
    Public Overrides Function ToString() As String
        Return hostType.ToString() & ": " & hostName & "(" & hostIP & ") " & hostState
    End Function
#End Region
End Class
