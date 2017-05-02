Public Interface IView
    Sub AddLogItem(item As String)
    Sub ClearList()
    'Sub ToggleLogging()
    Sub ShowErrorMessage(message As String, Optional errorlevel As EErrorLevel = EErrorLevel.None)
    Sub Dispatch(priority As Windows.Threading.DispatcherPriority, method As [Delegate])
    Function GetStatusMessage() As String
    Sub SetStatusMessage(message As String)
    Function GetStatusLogList() As IViewList
End Interface
