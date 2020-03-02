$Url = "https://localhost:5001/api/v1/account/create"

$Body = @{
    "UserName" = "test@email.io"
    "Email" = "test@email.io"
    "Password" = "Test_password1"
}

Invoke-RestMethod -Method Post -Uri $url -Body ($body|ConvertTo-Json) -ContentType "application/json"