Imports System.Reflection.Metadata
Imports SharpDX


Public Class Form1






    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ran As New Random

        Dim node(50, 50) As Vector2
        For x = 0 To 50
            For y = 0 To 50
                node(x, y) = Vector2.Normalize(New Vector2(ran.NextDouble, ran.NextDouble))
            Next
        Next
        Dim weight = Function(t_in As Single) As Single
                         Dim t As Single = 1.0 - t_in
                         Return 6.0 * (t ^ 5) - 15.0 * (t ^ 4) + 10.0 * (t ^ 3)
                     End Function

        Dim bitmap As New Bitmap(500, 500)
        'Using g As Graphics = Graphics.FromImage(bitmap)
        'End Using
        Dim data0(499, 499) As Single
        For x = 0 To 499
            For y = 0 To 499
                Dim d0 As Single = Vector2.Dot(node(Math.Floor(x / 50.0), Math.Floor(y / 50.0)), New Vector2(x - Math.Floor(x / 50.0) * 50.0, y - Math.Floor(y / 50.0) * 50.0) / 40)
                Dim d1 As Single = Vector2.Dot(node(Math.Floor(x / 50.0), Math.Ceiling(y / 50.0)), New Vector2(x - Math.Floor(x / 50.0) * 50.0, y - Math.Ceiling(y / 50.0) * 50.0) / 40)
                Dim d2 As Single = Vector2.Dot(node(Math.Ceiling(x / 50.0), Math.Floor(y / 50.0)), New Vector2(x - Math.Ceiling(x / 50.0) * 50.0, y - Math.Floor(y / 50.0) * 50.0) / 40)
                Dim d3 As Single = Vector2.Dot(node(Math.Ceiling(x / 50.0), Math.Ceiling(y / 50.0)), New Vector2(x - Math.Ceiling(x / 50.0) * 50.0, y - Math.Ceiling(y / 50.0) * 50.0) / 40)

                Dim x_prog As Single = x / 50.0 - Math.Floor(x / 50.0)
                Dim y_prog As Single = y / 50.0 - Math.Floor(y / 50.0)
                Dim tmp0 As Single = d0 * weight(y_prog) + d1 * weight(1.0 - y_prog)
                Dim tmp1 As Single = d2 * weight(y_prog) + d3 * weight(1.0 - y_prog)
                Dim v As Single = tmp0 * weight(x_prog) + tmp1 * weight(1.0 - x_prog)
                data0(x, y) = v
                'v = (v * 0.5 + 0.5) * 255
                'bitmap.SetPixel(x, y, Drawing.Color.FromArgb(v, v, v))
            Next
        Next
        For x = 0 To 499
            For y = 0 To 499
                Dim v As Single = data0(x, y)  ' data0(0.25 * x, 0.25 * y) + 0.8 * data0(0.5 * x, 0.5 * y) + 0.64 * data0(x, y)
                v = Math.Clamp(v, -1.0, 1.0)
                v = (v * 0.5 + 0.5) * 255
                bitmap.SetPixel(x, y, Drawing.Color.FromArgb(v, v, v))
            Next
        Next

        PictureBox1.Image = bitmap
        PictureBox1.Invalidate()
    End Sub

    ' not right!
    ' see https://mrl.cs.nyu.edu/~perlin/noise/



End Class
