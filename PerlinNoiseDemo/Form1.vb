Imports SharpDX


Public Class Form1

    Public ran As New Random

    Public node(10, 10) As Vector2

    Public bitmap As New Bitmap(500, 500)

    Public data0(499, 499) As Single

    Public UV_Shift As Vector2 = New Vector2(0, 0)


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For x = 0 To 9
            For y = 0 To 9
                Dim theta As Single = ran.NextDouble * 2.0 * Math.PI
                node(x, y) = Vector2.Normalize(New Vector2(Math.Cos(theta), Math.Sin(theta)))
            Next
        Next
        For tmp = 0 To 9
            node(tmp, 10) = node(tmp, 0)
            node(10, tmp) = node(0, tmp)
        Next
        node(10, 10) = node(0, 0)
        Dim fade = Function(t As Single) As Single
                       Return 6.0 * (t ^ 5) - 15.0 * (t ^ 4) + 10.0 * (t ^ 3)
                   End Function


        'Using g As Graphics = Graphics.FromImage(bitmap)
        'End Using
        For x = 0 To 499
            For y = 0 To 499
                Dim tile_x As Integer = Math.Floor(x / 50.0)
                Dim tile_y As Integer = Math.Floor(y / 50.0)

                Dim offset0 As Vector2 = New Vector2(x / 50.0 - tile_x, y / 50.0 - tile_y)
                Dim offset1 As Vector2 = New Vector2(x / 50.0 - tile_x - 1, y / 50.0 - tile_y)
                Dim offset2 As Vector2 = New Vector2(x / 50.0 - tile_x, y / 50.0 - tile_y - 1)
                Dim offset3 As Vector2 = New Vector2(x / 50.0 - tile_x - 1, y / 50.0 - tile_y - 1)

                Dim d0 As Single = Vector2.Dot(node(tile_x, tile_y), offset0)
                Dim d1 As Single = Vector2.Dot(node(tile_x + 1, tile_y), offset1)
                Dim d2 As Single = Vector2.Dot(node(tile_x, tile_y + 1), offset2)
                Dim d3 As Single = Vector2.Dot(node(tile_x + 1, tile_y + 1), offset3)

                Dim x_prog As Single = fade(x / 50.0 - tile_x)
                Dim y_prog As Single = fade(y / 50.0 - tile_y)
                Dim tmp0 As Single = d0 * (1.0 - x_prog) + d1 * x_prog
                Dim tmp1 As Single = d2 * (1.0 - x_prog) + d3 * x_prog
                Dim v As Single = tmp0 * (1.0 - y_prog) + tmp1 * y_prog
                data0(x, y) = v
                'v = (v * 0.5 + 0.5) * 255
                'bitmap.SetPixel(x, y, Drawing.Color.FromArgb(v, v, v))
            Next
        Next




    End Sub

    Public Function get_data(x_in As Single, y_in As Single) As Single
        Return data0(CInt(x_in) Mod 500, CInt(y_in) Mod 500)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Enabled = Not Timer1.Enabled
    End Sub

    Public Sub DrawImage(delta As Vector2)
        For sx = 0 To 499
            For sy = 0 To 499
                Dim x As Integer = sx + Math.Floor(delta.X)
                Dim y As Integer = sy + Math.Floor(delta.Y)

                ' Dim v As Single = data0(x, y)  
                Dim weight As Single = 0.6
                Dim v As Single = get_data(x, y) + weight * get_data(2.0 * x, 2.0 * y) + (weight ^ 2) * get_data(4.0 * x, 4.0 * y)
                v = Math.Clamp(v, -1.0, 1.0)
                v = (v * 0.5 + 0.5) * 255
                bitmap.SetPixel(sx, sy, Drawing.Color.FromArgb(v, v, v))
            Next
        Next

        PictureBox1.Image = bitmap
        PictureBox1.Invalidate()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        DrawImage(UV_Shift)
        UV_Shift += New Vector2(2.618, 1.0)
    End Sub

    ' see https://mrl.cs.nyu.edu/~perlin/noise/



End Class
