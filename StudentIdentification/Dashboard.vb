Public Class Dashboard

 
    Private Sub btn_addStudent_Click(sender As Object, e As EventArgs) Handles btn_addStudent.Click
        Try
            Dim addStud As New AddStudentvb
            addStud.Show()
            Me.Hide()
        Catch ex As Exception

        End Try
        

    End Sub

    Private Sub btn_updateStudent_Click(sender As Object, e As EventArgs) Handles btn_updateStudent.Click
        Try
            Dim updateStud As New UpdateStudent
            updateStud.Show()
            Me.Hide()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_studentList_Click(sender As Object, e As EventArgs) Handles btn_studentList.Click
        Try
            Dim StudList As New ListOfStudents
            StudList.Show()
            Me.Hide()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_scan_Click(sender As Object, e As EventArgs) Handles btn_scan.Click
        Try
            Dim scanner As New ScanQRCode
            scanner.Show()
            Me.Hide()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_dashboard_Click(sender As Object, e As EventArgs) Handles btn_dashboard.Click
        Try
            Dim dash As New Dashboard
            dash.Show()
            Me.Hide()

        Catch ex As Exception

        End Try
    End Sub
End Class
