namespace MVVMToolKit.Helper
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public static class CoreDumpHelper
    {
        [DllImport("Dbghelp.dll")]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, IntPtr hFile, int dumpType, ref MINIDUMP_EXCEPTION_INFORMATION exceptionParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int ClientPointers;
        }
        public enum MiniDumpType : int
        {
          MiniDumpNormal = 0x00000000,
          MiniDumpWithDataSegs = 0x00000001,
          MiniDumpWithFullMemory = 0x00000002, // 전체 덤프
          MiniDumpWithHandleData = 0x00000004,
          MiniDumpFilterMemory = 0x00000008,
          MiniDumpScanMemory = 0x00000010,
          MiniDumpWithUnloadedModules = 0x00000020,
          MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
          MiniDumpFilterModulePaths = 0x00000080,
          MiniDumpWithProcessThreadData = 0x00000100,
          MiniDumpWithPrivateReadWriteMemory = 0x00000200,
          MiniDumpWithoutOptionalData = 0x00000400,
          MiniDumpWithFullMemoryInfo = 0x00000800,
          MiniDumpWithThreadInfo = 0x00001000,
          MiniDumpWithCodeSegs = 0x00002000,
          MiniDumpWithoutAuxiliaryState = 0x00004000,
          MiniDumpWithFullAuxiliaryState = 0x00008000,
          MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
          MiniDumpIgnoreInaccessibleMemory = 0x00020000,
          MiniDumpWithTokenInformation = 0x00040000,
          MiniDumpWithModuleHeaders = 0x00080000,
          MiniDumpFilterTriage = 0x00100000,
          MiniDumpWithAvxXStateContext = 0x00200000,
          MiniDumpWithIptTrace = 0x00400000,
          MiniDumpScanInaccessiblePartialPages = 0x00800000,
          MiniDumpFilterWriteCombinedMemory,
          MiniDumpValidTypeFlags = 0x01ffffff
        };
        public static void CreateMemoryDump(MiniDumpType type, string dumpFilePath)
        {
            MINIDUMP_EXCEPTION_INFORMATION info = new ();
            info.ClientPointers = 1;
            info.ExceptionPointers = Marshal.GetExceptionPointers();
            info.ThreadId = GetCurrentThreadId();

            using FileStream file = new FileStream(dumpFilePath, FileMode.Create);

            MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), file.SafeFileHandle.DangerousGetHandle(), (int)type, ref info, IntPtr.Zero,  IntPtr.Zero);
        }
        public static void CreateMemoryDump()
        {
            var assembly = Assembly.GetEntryAssembly();
            var dirPath = Path.GetDirectoryName(assembly?.Location);
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            string dateTime = DateTime.Now.ToString("[yyyy-MM-dd][HH-mm-ss-fff]");
            var path = $"{dirPath}/[{exeName}]{dateTime}.dmp";
            CreateMemoryDump(MiniDumpType.MiniDumpNormal, path);
        }
    }
}
