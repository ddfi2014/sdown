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
    Public Function GetServerState() As List(Of String)
        Dim stateList As New List(Of String)()
        stateList.Add("This is a test.")
        stateList.Add("This tests the ServerTest.")
        stateList.Add("This is a DUMMY for the actual methods.")
        Return stateList
    End Function
#End Region
End Class
