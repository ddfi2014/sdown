Imports ServerDownClassLibrary
Public Class ViewList
    Implements IViewList

    Private list As List(Of String)

    Public Sub New(ByRef list As List(Of String))
        Me.list = list
    End Sub

    Public Function GetList() As Object Implements IViewList.GetList
        Return list
    End Function
End Class
