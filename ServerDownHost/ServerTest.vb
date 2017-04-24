Imports System.IO

Public Class ServerTest

#Region "Declarations"
    Private Shared instance As ServerTest = Nothing
#End Region

#Region "Singleton"
    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ServerTest
        If instance Is Nothing Then
            instance = New ServerTest()
        End If
        Return instance
    End Function
#End Region

#Region "Functionality"
#Region "Input"
    Public Shared Function GetHostList() As List(Of String)
        Dim serverList As New List(Of String)()
        Using sr As New StreamReader(Path.GetFullPath("server.ini"), Text.Encoding.UTF8)
            While Not sr.EndOfStream
                serverList.Add(sr.ReadLine)
            End While
            sr.Close()
        End Using
        Return serverList
    End Function
#End Region
#Region "Process"
    Public Shared Function GetServerStateStrings() As List(Of String)
        Server.Servers.Clear()
        Dim serverStateStringList As New List(Of String)()
        BuildServerList()
        Server.TestHosts()
        Server.Servers.ForEach(Sub(host) serverStateStringList.Add(host.ToString()))
        Return serverStateStringList
    End Function

    Private Shared Sub BuildServerList()
        Dim stateValues As String()
        For Each state In GetHostList()
            stateValues = state.Split(";")
            Server.Servers.Add(New Server(stateValues(0), stateValues(1), stateValues(2), stateValues(3)))
        Next
    End Sub
#End Region
#End Region

#Region "Test"
    Public Shared Function DBGTest() As List(Of String)
        Dim stateList As New List(Of String)()
        stateList.Add("This is a test.")
        stateList.Add("This tests the ServerTest.")
        stateList.Add("This is a DUMMY for the actual methods.")
        Return stateList
    End Function
#End Region
End Class
