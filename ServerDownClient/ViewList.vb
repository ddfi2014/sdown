Imports ServerDownClassLibrary
Public Class ViewList
    Implements IViewList

    Private listBox As ListBox = Nothing


    Public Sub New(ByRef listBox As ListBox)
        Me.listBox = listBox
    End Sub

    Public Function GetList() As Object Implements IViewList.GetList
        Return listBox
    End Function
End Class