Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WebBrowser1.Navigate(TextBox1.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
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
            labelPictureSize.Text = bmp.Width.ToString() & " x " & bmp.Height.ToString()
            bmp.Dispose()
            If IO.File.Exists(Destination) Then IO.File.Delete(Destination)
        End If
    End Sub

    Public Sub OnLineToLocal(source As String, Destination As String)

        If IO.File.Exists(Destination) Then IO.File.Delete(Destination)
        My.Computer.Network.DownloadFile(source, Destination)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim Source As String
        Dim Destination As String
        Source = ListBox1.SelectedItem
        Source = Replace(Source, "SX355", "SX1000")
        Destination = "C:\temp\img.jpg"
        If IO.File.Exists(Destination) Then IO.File.Delete(Destination)
        My.Computer.Network.DownloadFile(Source, Destination)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnCreateAd.Click

        DownloadSelectedImages()
        CreateBlankPng()

        Dim newBM As Bitmap = Nothing
        Dim imgBG As Bitmap = Nothing
        Dim imgBGb As Bitmap = Nothing
        Dim imgAmz As Bitmap = Nothing
        DefineBItmaps(newBM, imgBG, imgBGb, imgAmz)

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

        DisposeBitmaps(newBM, imgBG, imgBGb, imgAmz)

    End Sub

    Private Shared Sub DisposeBitmaps(newBM As Bitmap, imgBG As Bitmap, imgBGb As Bitmap, imgAmz As Bitmap)
        imgBG.Dispose()
        imgBGb.Dispose()
        imgAmz.Dispose()
        newBM.Dispose()
    End Sub

    Private Shared Sub DefineBItmaps(ByRef newBM As Bitmap, ByRef imgBG As Bitmap, ByRef imgBGb As Bitmap, ByRef imgAmz As Bitmap)
        newBM = New Bitmap(1200, 628)    'the "canvas" to draw on
        imgBG = Image.FromFile("C:\temp\blank.png")    'the background image
        imgBGb = Image.FromFile("C:\temp\blank-black.png")   'the black background image
        imgAmz = Image.FromFile("C:\temp\img.jpg")  'the amazon image
    End Sub

    Const DESTINATION As String = "C:\temp\img.jpg"

    Public Sub DownloadSelectedImages()
        Dim Source As String = Replace(ListBox1.SelectedItem, "SX355", "SX1000")
        If IO.File.Exists(DESTINATION) Then IO.File.Delete(DESTINATION)
        My.Computer.Network.DownloadFile(Source, DESTINATION)
    End Sub

    Public Sub CreateBlankPng()
        If IO.File.Exists("C:\temp\blank.png") = False Then
            Dim NewBitmap As New Bitmap(1200, 628, Imaging.PixelFormat.Format32bppPArgb)
            Graphics.FromImage(NewBitmap).Clear(Color.White)
            NewBitmap.Save("C:\temp\blank.png", Imaging.ImageFormat.Png)
        End If
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

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        If ListBox1.Items.Count < 1 Then
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
        End If
    End Sub
End Class
