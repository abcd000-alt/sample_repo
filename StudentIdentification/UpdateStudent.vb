Imports MySql.Data.MySqlClient
Imports ThoughtWorks.QRCode.Codec
Imports System.IO
Imports System.Globalization

Public Class UpdateStudent


    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click

        Dim connection As MySqlConnection = XammpConnection.GetConnection()
        Dim ms As New MemoryStream
        pb_qrCode.Image.Save(ms, pb_qrCode.Image.RawFormat)
        pb_qrCode.Image = Image.FromStream(ms)
        Try
            connection.Open()

            Dim query As String = "UPDATE tbl_studentinfo SET name = @name, birthdate = @birthdate, gender = @gender, grade = @grade, section = @section, contactNo = @contactNo, Address = @Address, qrCode = @qrCode WHERE IDno = @IDno"
            Dim command As New MySqlCommand(query, connection)
            'Parameters'
            command.Parameters.AddWithValue("@IDno", txt_ID.Text)
            command.Parameters.AddWithValue("@name", txt_name.Text)
            command.Parameters.AddWithValue("@birthdate", dtp_bday.Value.Date)
            command.Parameters.AddWithValue("@gender", If(cb_gender.SelectedItem IsNot Nothing, cb_gender.SelectedItem.ToString(), ""))
            command.Parameters.AddWithValue("@grade", If(cb_grade.SelectedItem IsNot Nothing, cb_grade.SelectedItem.ToString(), ""))
            command.Parameters.AddWithValue("@section", If(cb_section.SelectedItem IsNot Nothing, cb_section.SelectedItem.ToString(), ""))
            'command.Parameters.AddWithValue("@gender", cb_gender.SelectedItem.ToString)
            'command.Parameters.AddWithValue("@grade", cb_grade.SelectedItem.ToString)
            'command.Parameters.AddWithValue("@section", cb_section.SelectedItem.ToString)
            command.Parameters.AddWithValue("@contactNo", txt_CNo.Text)
            command.Parameters.AddWithValue("@Address", txt_address.Text)
            command.Parameters.Add("@qrCode", MySqlDbType.Blob).Value = ms.ToArray()




            command.ExecuteNonQuery()

            MessageBox.Show("Data has been saved to the database", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            dgv_table.Rows.Clear()
            getAllInfo()
            eraseInfo()

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

    Private Sub getAllInfo()

        Dim connection As MySqlConnection = XammpConnection.GetConnection()

        Try
            connection.Open()

            Dim query As String = "SELECT * FROM tbl_studentinfo"
            Dim command As MySqlCommand = New MySqlCommand(query, connection)

            Dim reader As MySqlDataReader = command.ExecuteReader()


            If reader.HasRows Then
                While reader.Read()
                    Dim row As DataGridViewRow = New DataGridViewRow()
                    row.CreateCells(dgv_table)
                    row.Cells(0).Value = reader("IDno")
                    row.Cells(1).Value = reader("name")
                    dgv_table.Columns(2).DefaultCellStyle.Format = "dd/MM/yyyy"
                    row.Cells(2).Value = reader("birthdate")
                    row.Cells(3).Value = reader("gender")
                    row.Cells(4).Value = reader("grade")
                    row.Cells(5).Value = reader("section")
                    row.Cells(6).Value = reader("contactNo")
                    row.Cells(7).Value = reader("Address")

                    'Retrieve QR code data from the database
                    If Not IsDBNull(reader("qrCode")) Then
                        Dim qrCodeBytes As Byte() = CType(reader("qrCode"), Byte())

                        ' Convert byte array to Image
                        Dim qrCodeImage As Image = Image.FromStream(New MemoryStream(qrCodeBytes))

                        ' Set the QR code image to the PictureBox
                        pb_qrCode.Image = qrCodeImage
                    Else
                        ' Handle the case where qrCode is NULL (optional)
                        pb_qrCode.Image = Nothing
                    End If
                  
                    dgv_table.Rows.Add(row)
                End While
            End If

            reader.Close()

        Catch ex As Exception
            MessageBox.Show("An error occures while saving the data" & ex.Message)
        Finally
            connection.Close()
        End Try

    End Sub
    
    Private Sub dgv_table_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_table.CellContentClick
        
        Try
            Dim index As Integer

            index = e.RowIndex
            Dim selectedRow As DataGridViewRow
            selectedRow = dgv_table.Rows(index)

            txt_ID.Text = selectedRow.Cells(0).Value.ToString
            txt_name.Text = selectedRow.Cells(1).Value.ToString
            dtp_bday.Value = selectedRow.Cells(2).Value.ToString
            cb_gender.Text = selectedRow.Cells(3).Value.ToString
            cb_grade.Text = selectedRow.Cells(4).Value.ToString
            cb_section.Text = selectedRow.Cells(5).Value.ToString
            txt_CNo.Text = selectedRow.Cells(6).Value.ToString
            txt_address.Text = selectedRow.Cells(7).Value.ToString

            '' Retrieve QR code data from the selected row in DataGridView
            'If Not IsDBNull(selectedRow.Cells("qrCode").Value) Then
            '    Dim qrCodeBytes As Byte() = CType(selectedRow.Cells("qrCode").Value, Byte())

            '    ' Convert byte array to Image
            '    Dim qrCodeImage As Image = Image.FromStream(New MemoryStream(qrCodeBytes))

            '    ' Set the QR code image to the PictureBox
            '    pb_qrCode.Image = qrCodeImage
            'Else
            '    ' Handle the case where qrCode is NULL (optional)
            '    pb_qrCode.Image = Nothing
            'End If

            'UpdatePictureBoxValue()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub eraseInfo()
        txt_ID.Text = ""
        txt_name.Text = ""
        dtp_bday.Value = DateTime.Now
        txt_CNo.Text = ""
        txt_address.Text = ""
        cb_gender.SelectedItem = Nothing
        cb_grade.SelectedItem = Nothing
        cb_section.SelectedItem = Nothing
        pb_qrCode.Image = Nothing

    End Sub

    Private Sub UpdateStudent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtp_bday.Format = DateTimePickerFormat.Custom
        dtp_bday.CustomFormat = "dd/MM/yyyy"
        getAllInfo()
        ' UpdatePictureBoxValue()


    End Sub

    'Private Sub UpdatePictureBoxValue()
    '    ' Retrieve QR code data from the selected row in DataGridView
    '    If dgv_table.CurrentRow IsNot Nothing AndAlso Not IsDBNull(dgv_table.CurrentRow.Cells("qrCode").Value) Then
    '        Dim qrCodeBytes As Byte() = CType(dgv_table.CurrentRow.Cells("qrCode").Value, Byte())

    '        ' Check if the byte array is not empty
    '        If qrCodeBytes IsNot Nothing AndAlso qrCodeBytes.Length > 0 Then
    '            Try
    '                ' Attempt to convert byte array to Image
    '                Dim qrCodeImage As Image = Image.FromStream(New MemoryStream(qrCodeBytes))

    '                ' Set the QR code image to the PictureBox
    '                pb_qrCode.Image = qrCodeImage
    '            Catch ex As Exception
    '                ' Handle the case where the byte array does not represent a valid image format
    '                MessageBox.Show("Error loading image: " & ex.Message)
    '                pb_qrCode.Image = Nothing
    '            End Try
    '        Else
    '            ' Handle the case where the byte array is empty
    '            pb_qrCode.Image = Nothing
    '        End If
    '    Else
    '        ' Handle the case where qrCode is NULL (optional)
    '        pb_qrCode.Image = Nothing
    '    End If
    'End Sub

   
    Private Sub txt_search_TextChanged(sender As Object, e As EventArgs) Handles txt_search.TextChanged

    End Sub
End Class