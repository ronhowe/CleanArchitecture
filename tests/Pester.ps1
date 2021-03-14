Describe "The Application" {
    Context "When Started" -Tag "Online" {
        BeforeAll {
            $Uri = "https://localhost:5001/api/WeatherForecast"

            $Response = Invoke-WebRequest -Uri $Uri -Method Get # -SkipHttpErrorCheck -SkipCertificateCheck

            # Quell PSScriptAnalyzer warning about a variable is assigned but never used.
            $Response | Out-Null
        }
        It "Should Return 200" {
            $Response.StatusCode | Should -Be 200
        }
        AfterAll {
            Write-Host "Uri Tested = $Uri" -ForegroundColor DarkYellow
        }
    }
}
