Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WebBrowser1.Navigate(TextBox1.Text)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim htmlDocument As HtmlDocument = Me.WebBrowser1.Document
        Dim htmlElementCollection As HtmlElementCollection = htmlDocument.Images
        For Each htmlElement As HtmlElement In htmlElementCollection
            Dim imgUrl As String = htmlElement.GetAttribute("src")
            If imgUrl.Contains("jpg") Or imgUrl.Contains("png") Then
                ListBox1.Items.Add(imgUrl)
            End If

        Next
        WebBrowser1.Visible = False
        PictureBox1.Visible = True
        ListBox1.SelectedIndex = 0
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        PictureBox1.ImageLocation = ListBox1.SelectedItem
        If ListBox1.SelectedIndex > 0 Then
            Dim Destination As String = "C:\temp\imgtmp.jpg"
            OnLineToLocal(ListBox1.SelectedItem.ToString, Destination)
            Dim bmp As New Bitmap("C:\temp\imgtmp.jpg")

            Label3.Text = bmp.Width.ToString()
            bmp.Dispose()
            If System.IO.File.Exists(Destination) Then System.IO.File.Delete(Destination)
        End If


    End Sub

    Public Sub OnLineToLocal(source As String, Destination As String)

        If System.IO.File.Exists(Destination) Then System.IO.File.Delete(Destination)
        My.Computer.Network.DownloadFile(source, Destination)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim Source As String
        Dim Destination As String
        Source = ListBox1.SelectedItem
        Source = Replace(Source, "SX355", "SX1000")
        Destination = "C:\temp\img.jpg"
        If System.IO.File.Exists(Destination) Then System.IO.File.Delete(Destination)
        My.Computer.Network.DownloadFile(Source, Destination)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Source As String
        Dim Destination As String
        Source = ListBox1.SelectedItem
        Source = Replace(Source, "SX355", "SX1000")
        Destination = "C:\temp\img.jpg"
        If System.IO.File.Exists(Destination) Then System.IO.File.Delete(Destination)
        My.Computer.Network.DownloadFile(Source, Destination)

        If System.IO.File.Exists("C:\temp\blank.png") = False Then
            Dim NewBitmap As New Bitmap(1200, 628, Imaging.PixelFormat.Format32bppPArgb)
            Graphics.FromImage(NewBitmap).Clear(Color.White)
            NewBitmap.Save("C:\temp\blank.png", Imaging.ImageFormat.Png)
        End If
        Dim newBM As Bitmap     'the "canvas" to draw on
        Dim imgBG As Bitmap     'the background image
        Dim imgBGb As Bitmap    'the black background image
        Dim imgAmz As Bitmap   'the amazon image
        imgBG = Image.FromFile("C:\temp\blank.png")
        imgBGb = Image.FromFile("C:\temp\blank-black.png")
        imgAmz = Image.FromFile("C:\temp\img.jpg")
        newBM = New Bitmap(1200, 628)
        Dim NewHeight As Integer = 628
        Dim NewWidth As Integer = NewHeight / imgAmz.Height * imgAmz.Width

        If NewWidth > 1200 Then
            NewWidth = 1200
            NewHeight = NewWidth / imgAmz.Width * imgAmz.Height
        End If

        imgAmz = New Bitmap(imgAmz, NewWidth, NewHeight)

        Dim Xpt As Integer = 600 - (NewWidth / 2)

        Graphics.FromImage(newBM).DrawImage(imgBG, 0, 0)
        Graphics.FromImage(newBM).DrawImage(imgAmz, Xpt, 0)

        newBM.Save("C:\temp\image01.png", Imaging.ImageFormat.Png)
        PictureBox1.Image = New Bitmap(newBM)
        Graphics.FromImage(newBM).DrawImage(imgBGb, 0, 0)
        Graphics.FromImage(newBM).DrawImage(imgAmz, Xpt, 0)
        newBM.Save("C:\temp\image02.png", Imaging.ImageFormat.Png)

        DisplayMetaDescription()

        imgBG.Dispose()
        imgBGb.Dispose()
        imgAmz.Dispose()
        newBM.Dispose()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Dim Directory As String = TextBox1.Text
        DisplayMetaDescription()
    End Sub
    Private Sub DisplayMetaDescription()
        If (WebBrowser1.Document IsNot Nothing) Then
            Dim Elems As HtmlElementCollection
            Dim WebOC As WebBrowser = WebBrowser1

            Elems = WebOC.Document.GetElementsByTagName("META")

            For Each elem As HtmlElement In Elems
                Dim NameStr As String = elem.GetAttribute("name")

                If ((NameStr IsNot Nothing) And (NameStr.Length <> 0)) Then
                    If NameStr.ToLower().Equals("description") Then
                        Dim ContentStr As String = elem.GetAttribute("content")
                        'MessageBox.Show("Document: " & WebOC.Url.ToString() & vbCrLf & "Description: " & ContentStr)
                        TextBoxDescription.Text = ContentStr
                    End If
                    If NameStr.ToLower().Equals("title") Then
                        Dim ContentStr As String = elem.GetAttribute("content")
                        'MessageBox.Show("Document: " & WebOC.Url.ToString() & vbCrLf & "Title: " & ContentStr)
                        TextBoxTitle.Text = ContentStr
                    End If
                End If

            Next
        End If
    End Sub
End Class
