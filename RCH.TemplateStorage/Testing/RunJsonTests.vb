''' ============================================================================
''' File: RunJsonTests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Test runner for JSON serialization unit tests
''' Created: 2026-01-05
''' ============================================================================

Imports RCH.TemplateStorage.Testing

Module RunJsonTests

    Sub Main()
        Console.WriteLine("RCH.TemplateStorage - JSON Serialization Test Runner")
        Console.WriteLine()

        Try
            JsonSerializationTests.RunAllTests()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"Fatal Error: {ex.Message}")
            Console.WriteLine(ex.StackTrace)
            Console.ResetColor()
        End Try

        Console.WriteLine()
        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub

End Module
