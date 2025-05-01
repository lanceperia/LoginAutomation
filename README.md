## Prerequisites:
This console application requires specific configurations before running. These configurations utilize system environment variables.  

Below is the PowerShell script you can use to add environment variables:

``` Powershell

# Define environment variables
$envVars = @{

    # Needed to setup chrome profile.
    "CHROME_USERDATADIR" = "C:\Users\AppData\Local\Google\Chrome\User Data"
    "CHROME_PROFILEDIR" = "Profile 1"

    # Required if email provider is Mailgun
    "MAILGUN_ENDPOINT" = "your_mailgun_endpoint"
    "MAILGUN_APIKEY" = "your_mailgun_api_key"
    "MAILGUN_RECIPIENT" = "recipient_email"
    "MAILGUN_SENDER" = "sender_email"

    # Required if email provider is AWS SNS
    "PERSONAL_AWS_ACCESS_KEY_ID" = "your_access_key_id"
    "PERSONAL_AWS_SECRET_ACCESS_KEY" = "your_secret_access_key"
    "PERSONAL_AWS_SNS_ARN" = "your_aws_sns_arn"
}

# Set environment variables
foreach ($key in $envVars.Keys) {
    [System.Environment]::SetEnvironmentVariable($key, $envVars[$key], [System.EnvironmentVariableTarget]::User)
}
```
Both of these can be found by navigating to `chrome://version/`:  

**CHROME_USERDATADIR**: ![image](https://github.com/user-attachments/assets/b5bea65b-33fb-4363-b62e-274667753d92)

**CHROME_PROFILEDIR**: ![image](https://github.com/user-attachments/assets/3db5f616-c17c-4d71-b584-704322a1a025)
