Imports MySql.Data.MySqlClient
Imports ThoughtWorks.QRCode.Codec
Imports System.IO
Public Class AddStudentvb

    Private Sub pb_addStudent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pb_addStudent.Click

        Dim ms As New MemoryStream
        pb_qrCode.Image.Save(ms, pb_qrCode.Image.RawFormat)
        pb_qrCode.Image = Image.FromStream(ms)

        Dim connection As MySqlConnection = XammpConnection.GetConnection()

        Try
            connection.Open()

            Dim query As String = " INSERT INTO tbl_studentinfo(name, birthdate, gender, grade, section, contactNo, Address, qrCode ) VALUE( @name, @birthdate, @gender, @grade, @section, @contactNo, @Address, @qrCode )"
            Dim command As New MySqlCommand(query, connection)
            'Parameters'

            command.Parameters.AddWithValue("@name", txt_name.Text)
            command.Parameters.AddWithValue("@birthdate", dtp_bday.Value.Date)
            command.Parameters.AddWithValue("@gender", cb_gender.SelectedItem.ToString)
            command.Parameters.AddWithValue("@grade", cb_grade.SelectedItem.ToString)
            command.Parameters.AddWithValue("@section", cb_section.SelectedItem.ToString)
            command.Parameters.AddWithValue("@contactNo", txt_CNo.Text)
            command.Parameters.AddWithValue("@Address", txt_address.Text)
            command.Parameters.Add("@qrCode", MySqlDbType.Blob).Value = ms.ToArray()




            command.ExecuteNonQuery()
            MessageBox.Show("Data has been saved to the database", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            MessageBox.Show("An error occures while saving the data" & ex.Message)
        Finally
            connection.Close()
        End Try
    
    End Sub

    Private Sub btn_generate_Click(sender As Object, e As EventArgs) Handles btn_generate.Click

    Dim objqrcode As QRCodeEncoder = New QRCodeEncoder
    Dim img As Image
    Dim btm As Bitmap
    Dim str As String
        Try
    Dim thk As String = "     Thank you:"
            str = txt_name.Text + " " + dtp_bday.Value + " " + cb_gender.SelectedItem + " " + cb_grade.SelectedItem + " " + cb_section.SelectedItem + " " + txt_CNo.Text + " " + txt_address.Text + thk
            objqrcode.QRCodeScale = 5
            img = objqrcode.Encode(str)
            btm = New Bitmap(img)
            btm.Save("qrimage.jpg")
            pb_qrCode.ImageLocation = "qrimage.jpg"



        Catch ex As Exception

        End Try
    End Sub
    Private Sub Save_QR()
        Try
            ' Use SaveFileDialog to get the file path
            SFD.Filter = "PNG Image | *.png"
            SFD.FileName = "QR." & txt_name.Text & ".png" 'instead na name ang namefile himoon natong id no.

            If SFD.ShowDialog() = DialogResult.OK Then
                ' Save the QR code as a PNG image
                Dim img As New Bitmap(PictureBox1.Image)
                img.Save(SFD.FileName, Imaging.ImageFormat.Png)
                MsgBox("QR Code Successfully Saved!!!")
            End If
        Catch ex As Exception
            ' Handle exceptions here
            MessageBox.Show("Error saving QR code: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btn_saveQR_Click(sender As Object, e As EventArgs) Handles btn_saveQR.Click
        Save_QR()

    End Sub

  
    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub
End Class