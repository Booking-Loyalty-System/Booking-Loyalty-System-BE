using System.Text.RegularExpressions;

// Lấy tên nhánh hiện tại từ Git
var process = new System.Diagnostics.Process
{
    StartInfo = new System.Diagnostics.ProcessStartInfo
    {
        FileName = "git",
        Arguments = "rev-parse --abbrev-ref HEAD",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    }
};
process.Start();
string branchName = process.StandardOutput.ReadToEnd().Trim();

// Định nghĩa chuẩn tên nhánh (Regex)
// Chuẩn: feature/..., bugfix/..., hotfix/..., release/...
var regex = @"^(feature|bugfix|hotfix|release)\/.+";

if (!Regex.IsMatch(branchName, regex) && branchName != "main" && branchName != "master")
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ LỖI: Tên nhánh sai định dạng!");
    Console.WriteLine($"Tên hiện tại: {branchName}");
    Console.WriteLine("Quy tắc: feature/..., bugfix/..., hotfix/..., release/...");
    Console.ResetColor();
    return 1;
}

Console.WriteLine($"✅ Tên nhánh '{branchName}' hợp lệ.");
return 0;