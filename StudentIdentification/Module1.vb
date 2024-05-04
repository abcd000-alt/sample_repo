Imports MySql.Data.MySqlClient

Module XammpConnection
    Public str_name, str_position, str_uname, str_pword As String
    Public Function GetConnection() As MySqlConnection
        Dim connectionstring As String = "SERVER = localhost; USER ID = root; password = ; database = student_identification_card; "
        Dim connection As MySqlConnection = Nothing

        Try
            connection = New MySqlConnection(connectionstring)
        Catch ex As Exception
            MessageBox.Show("Error Connecting to database" & ex.Message)
        End Try
        Return connection
    End Function

End Module
