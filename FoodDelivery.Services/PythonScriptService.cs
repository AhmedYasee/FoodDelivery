using System.Diagnostics;

namespace FoodDelivery.Services
{
    public class PythonScriptService
    {
        public void RunPythonScript(string scriptName)
        {
            // Define the Python interpreter and script path
            string pythonExe = "python"; // Ensure Python is in PATH or provide the full path to python.exe
            string scriptPath = $"C:\\Users\\yasein\\Downloads\\FoodDelivery\\FoodDelivery\\FoodDelivery.Services\\Ai\\{scriptName}";

            // Start a new process to execute the Python script
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processInfo })
            {
                process.Start();

                // Read the standard output and error (for debugging purposes)
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Throw an exception if there's an error in the Python script
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Python script failed: {error}");
                }

                // Optionally, log the script output (for debugging)
                Debug.WriteLine(output);
            }
        }
    }
}
