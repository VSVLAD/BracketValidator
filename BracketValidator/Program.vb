Module Program

    Public Sub Main()
        Dim BV As New BracketValidator

        For Each xText In Split("function(){ Верный }
                                (([[Верный]]))
                                (({()()[({({Верный})})]}))
                                (({()()[({({Верный})})]}))
                                (({()()[({({Верный})})]}))
                                (({()()[({({Ложный})})]})[)
                                (({()()[({({Ложный}))]}))
                                (([Ложный)])
                                )(Ложный)", vbCrLf)

            Console.WriteLine($"Проверка строки: {xText.Trim()}")
            Console.WriteLine($"Результат: {BV.Validate(xText)}")

            Console.WriteLine()
            Console.WriteLine()
        Next

        Console.ReadLine()
    End Sub

End Module


Public Class BracketValidator

    Private Structure BracketChar
        Public OpenChar As String
        Public CloseChar As String
    End Structure

    'Список скобок
    Private listBrackets As New List(Of BracketChar)

    'Конструктор
    Public Sub New()
        listBrackets.Add(New BracketChar() With {.OpenChar = "(", .CloseChar = ")"})
        listBrackets.Add(New BracketChar() With {.OpenChar = "[", .CloseChar = "]"})
        listBrackets.Add(New BracketChar() With {.OpenChar = "{", .CloseChar = "}"})
        listBrackets.Add(New BracketChar() With {.OpenChar = "<", .CloseChar = ">"})
    End Sub


    ''' <summary>Метод валидации</summary>
    Public Function Validate(Text As String) As Boolean
        Dim stackBrackets As New Stack(Of String)

        For Each tokenChar In Text

            If IsOpenBracket(tokenChar) Then
                stackBrackets.Push(tokenChar)

            ElseIf IsCloseBracket(tokenChar) Then

                'Если появилась закрытая скобка и нет ранее открытой выходим
                If stackBrackets.Count = 0 Then Return False

                'Если появилась закрытая скобка и она не относится к семейству открытой скобки из стека, то выходим
                If Not IsBracketFamily(stackBrackets.Pop(), tokenChar) Then Return False

            End If
        Next

        'Если есть остаточные скобки, значит не все закрылись
        If stackBrackets.Count > 0 Then Return False

        'Успех
        Return True
    End Function


    ''' <summary>Проверка скобок на отношение к одному типу</summary>
    Private Function IsBracketFamily(BracketOpen As String, BracketClose As String) As Boolean
        For Each Br In listBrackets
            If Br.OpenChar = BracketOpen AndAlso Br.CloseChar = BracketClose Then Return True
        Next
        Return False
    End Function

    ''' <summary>Проверяет, является ли символ открытой скобкой</summary>
    Private Function IsOpenBracket(TokenChar) As Boolean
        For Each Br In listBrackets
            If Br.OpenChar = TokenChar Then Return True
        Next
        Return False
    End Function

    ''' <summary>Проверяет, является ли символ закрытой скобкой</summary>
    Private Function IsCloseBracket(TokenChar) As Boolean
        For Each Br In listBrackets
            If Br.CloseChar = TokenChar Then Return True
        Next
        Return False
    End Function
End Class