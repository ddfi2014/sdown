Imports ServerDownClassLibrary

Class MainWindow
    Implements IView
#Region "Declaration"
    Private control As ServerDownHostControl = Nothing
    Public Property StatusMessage() As String
        Get
            Return statusBarItemMessage.Content
        End Get
        Set(value As String)
            statusBarItemMessage.Content = value
        End Set
    End Property
#End Region
#Region "Initialization"
    ''' <summary>
    ''' Registers the view at a control object.
    ''' </summary>
    Private Sub SetControl()
        Try
            control = ServerDownHostControl.GetInstance()
            ServerDownHostControl.AddView(Me)
        Catch ex As Exception
            Console.WriteLine("ERROR: Couldn't set control to ServerDownHostControl.")
            Close()
        End Try
    End Sub
#End Region
#Region "Implementation"
    Public Sub AddLogItem(item As String) Implements IView.AddLogItem
        listBoxLog.Items.Add(item)
    End Sub

    Public Sub ClearList() Implements IView.ClearList
        listBoxLog.Items.Clear()
    End Sub

    'Public Sub ToggleLogging() Implements IView.ToggleLogging
    '    If checkBoxLog.IsChecked Then
    '        checkBoxLog.IsChecked = False
    '    Else
    '        checkBoxLog.IsChecked = True
    '    End If
    'End Sub

    Public Sub ShowErrorMessage(message As String, Optional errorlevel As EErrorLevel = EErrorLevel.None) Implements IView.ShowErrorMessage
        Select Case errorlevel
            Case EErrorLevel.None

            Case EErrorLevel.Hint
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK)
            Case EErrorLevel.Warning
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)
            Case EErrorLevel.CriticalError
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK)
            Case Else

        End Select
    End Sub
#End Region
#Region "Events"
    Private Sub buttonStart_Click(sender As Object, e As RoutedEventArgs) Handles buttonStart.Click
        control.InitializeHost()
    End Sub

    Private Sub buttonStop_Click(sender As Object, e As RoutedEventArgs) Handles buttonStop.Click
        control.CloseHost()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetControl()
    End Sub

    'Private Sub checkBoxLog_Checked(sender As Object, e As RoutedEventArgs) Handles checkBoxLog.Checked
    '    control.SetLogOn()
    'End Sub

    'Private Sub checkBoxLog_Unchecked(sender As Object, e As RoutedEventArgs) Handles checkBoxLog.Unchecked
    '    control.SetLogOff()
    'End Sub

    'Private Sub checkBoxSaveTest_Checked(sender As Object, e As RoutedEventArgs) Handles checkBoxSave.Checked
    '    control.StartSaveTimer()
    'End Sub

    'Private Sub checkBoxSaveTest_Unchecked(sender As Object, e As RoutedEventArgs) Handles checkBoxSave.Unchecked
    '    control.StopSaveTimer()
    'End Sub

    Private Sub buttonNewWindow_Click(sender As Object, e As RoutedEventArgs) Handles buttonNewWindow.Click
        Dim testW As New MainWindow()
        testW.Show()
    End Sub
#End Region
End Class