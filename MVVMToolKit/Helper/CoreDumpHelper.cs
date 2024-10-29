using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// Crash 시 Core Dump를 도와주기 위한 클래스입니다.
    /// </summary>
    public static class CoreDumpHelper
    {
        [DllImport("Dbghelp.dll")]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeFileHandle hFile, int dumpType, ref MinidumpExceptionInformation exceptionParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct MinidumpExceptionInformation
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int ClientPointers;
        }
        /// <summary>
        /// MiniDump를 생성하기 위한 타입입니다.
        /// </summary>
        public enum MiniDumpType
        {
            /// <summary>
            /// 프로세스의 모든 기존 스레드에 대한 스택 추적을 캡처하는 데 필요한 정보만 포함합니다.
            /// </summary>
            MiniDumpNormal = 0x00000000,
            /// <summary>
            /// 로드된 모든 모듈의 데이터 섹션을 포함합니다. 이로 인해 전역 변수가 포함되어
            /// 미니덤프 파일이 상당히 커질 수 있습니다. 모듈별 제어의 경우 MODULE_WRITE_FLAGS 의
            /// ModuleWriteDataSeg 열거 값을 사용하세요 .
            /// </summary>
            MiniDumpWithDataSegs = 0x00000001,
            /// <summary>
            /// 프로세스에 액세스 가능한 모든 메모리를 포함합니다. 마지막에는 원시 메모리 데이터가 포함되어 있어
            /// 원시 메모리 정보 없이 초기 구조를 직접 매핑할 수 있습니다. 이 옵션을 사용하면 파일 크기가 매우 커질 수 있습니다
            /// </summary>
            MiniDumpWithFullMemory = 0x00000002, // 전체 덤프
            /// <summary>
            /// 미니덤프가 만들어질 때 활성화된 운영 체제 핸들에 대한 상위 수준 정보를 포함합니다
            /// </summary>
            MiniDumpWithHandleData = 0x00000004,
            /// <summary>
            /// 미니덤프 파일에 기록된 스택 및 백업 저장소 메모리는
            /// 스택 추적을 재구성하는 데 필요한 포인터 값을 제외한 모든 항목을 제거하도록 필터링되어야 합니다 .
            /// </summary>
            MiniDumpFilterMemory = 0x00000008,
            /// <summary>
            /// 스택 및 백업 저장소 메모리는 모듈 목록의 모듈에 대한 포인터 참조를 검색해야 합니다.
            /// 모듈이 스택 또는 백업 저장소 메모리에 의해 참조되는 경우 MINIDUMP_CALLBACK_OUTPUT 구조 의 ModuleWriteFlags 멤버가 ModuleReferencedByMemory 로 설정 됩니다 .
            /// </summary>
            MiniDumpScanMemory = 0x00000010,
            /// <summary>
            /// 최근에 언로드된 모듈 목록의 정보를 포함합니다(이 정보가
            /// 운영 체제에서 유지 관리되는 경우).
            /// Windows Server 2003 및 Windows XP:   운영 체제는
            /// Windows Server 2003 SP1 및 Windows XP SP2까지 언로드된 모듈에 대한 정보를 유지하지 않습니다.
            /// </summary>
            MiniDumpWithUnloadedModules = 0x00000020,
            /// <summary>
            /// 로컬 또는 기타 스택 메모리에서 참조하는 데이터가 있는 페이지를 포함합니다. 이 옵션을 사용하면
            /// 미니덤프 파일 의 크기가 크게 늘어날 수 있습니다 .
            /// </summary>
            MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
            /// <summary>
            /// 사용자 이름이나 중요한 디렉터리와 같은 정보에 대한 모듈 경로를 필터링합니다. 이 옵션을 사용하면
            /// 시스템이 이미지 파일을 찾지 못할 수 있으므로 특별한 상황에서만 사용해야 합니다.
            /// </summary>
            MiniDumpFilterModulePaths = 0x00000080,
            /// <summary>
            /// 운영 체제의 전체 프로세스별 및 스레드별 정보를 포함합니다.
            /// </summary>
            MiniDumpWithProcessThreadData = 0x00000100,
            /// <summary>
            /// 포함할 PAGE_READWRITE 메모리 에 대한 가상 주소 공간을 검색합니다 .
            /// </summary>
            MiniDumpWithPrivateReadWriteMemory = 0x00000200,
            /// <summary>
            /// 덤프에 지정된 기준을 충족하는 데 필수적이지 않은 메모리 영역을 제거하여 덤프되는 데이터를 줄입니다 . 이렇게 하면 사용자에게 개인적인 데이터가 포함될 수 있는 메모리 덤프를 방지할 수 있습니다.
            /// 그러나 개인정보가 전혀 존재하지 않을 것이라는 보장은 없습니다.
            /// </summary>
            MiniDumpWithoutOptionalData = 0x00000400,
            /// <summary>
            /// 메모리 영역 정보를 포함합니다. 자세한 내용은
            /// MINIDUMP_MEMORY_INFO_LIST 를 참조하세요 .
            /// </summary>
            MiniDumpWithFullMemoryInfo = 0x00000800,
            /// <summary>
            /// 스레드 상태 정보를 포함합니다. 자세한 내용은
            /// MINIDUMP_THREAD_INFO_LIST 를 참조하세요 .
            /// </summary>
            MiniDumpWithThreadInfo = 0x00001000,
            /// <summary>
            /// 로드된 모듈의 모든 코드 및 코드 관련 섹션을 포함하여 실행 가능한 콘텐츠를 캡처합니다.
            /// 모듈별 제어 의 경우 MODULE_WRITE_FLAGS 의 ModuleWriteCodeSegs 열거 값을 사용하세요 . DbgHelp 6.1 이하:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithCodeSegs = 0x00002000,
            /// <summary>
            /// 보조 보조 지원 메모리 수집을 끕니다.
            /// </summary>
            MiniDumpWithoutAuxiliaryState = 0x00004000,
            /// <summary>
            /// 보조 데이터 제공자가 덤프 이미지에 해당 상태를 포함하도록 요청합니다. 포함 된 상태 데이터는
            /// 공급자에 따라 다릅니다. 이 옵션을 사용하면 대용량 덤프 이미지가 생성될 수 있습니다.
            /// </summary>
            MiniDumpWithFullAuxiliaryState = 0x00008000,
            /// <summary>
            /// 포함될 PAGE_WRITECOPY 메모리 에 대한 가상 주소 공간을 검색합니다 .
            /// 
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
            /// <summary>
            /// MiniDumpWithFullMemory 를 지정하면 함수가 메모리 영역을 읽을 수 없으면 MiniDumpWriteDump
            /// 함수 가 실패합니다 . 그러나 MiniDumpIgnoreInaccessibleMemory를
            /// 포함하면 MiniDumpWriteDump 함수 는 메모리 읽기 실패를 무시하고 계속해서 덤프를 생성합니다. 액세스할 수 없는 메모리 영역은 덤프 에 포함되지 않습니다 . DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpIgnoreInaccessibleMemory = 0x00020000,
            /// <summary>
            /// 보안 토큰 관련 데이터를 추가합니다. 이렇게 하면
            /// 사용자 모드 덤프를 처리할 때 "!token" 확장이 작동하게 됩니다 .
            /// 
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithTokenInformation = 0x00040000,
            /// <summary>
            /// 모듈 헤더 관련 데이터를 추가합니다.
            /// 
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithModuleHeaders = 0x00080000,
            /// <summary>
            /// 필터 분류 관련 데이터를 추가합니다.
            /// 
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpFilterTriage = 0x00100000,
            /// <summary>
            /// AVX 충돌 상태 컨텍스트 레지스터를 추가합니다.
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithAvxXStateContext = 0x00200000,
            /// <summary>
            /// Intel 프로세서 추적 관련 데이터를 추가합니다.
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpWithIptTrace = 0x00400000,
            /// <summary>
            /// 액세스할 수 없는 부분 메모리 페이지를 검색합니다.
            /// 
            /// DbgHelp 6.1 이전:   이 값은 지원되지 않습니다.
            /// </summary>
            MiniDumpScanInaccessiblePartialPages = 0x00800000,
            /// <summary>
            /// 
            /// </summary>
            MiniDumpFilterWriteCombinedMemory,
            /// <summary>
            /// 어떤 플래그가 유효한지 나타냅니다.
            /// </summary>
            MiniDumpValidTypeFlags = 0x01ffffff
        };
        /// <summary>
        /// Memory Dump를 생성합니다
        /// </summary>
        /// <param name="type">Mini Dump 타입</param>
        /// <param name="dumpFilePath">덤프를 저장할 위치</param>
        public static void CreateMemoryDump(MiniDumpType type, string dumpFilePath)
        {
            MinidumpExceptionInformation info = new()
            {
                ClientPointers = 1,
                ExceptionPointers = Marshal.GetExceptionPointers(),
                ThreadId = GetCurrentThreadId()
            };

            using (FileStream file = new(dumpFilePath, FileMode.Create))
            {
                var handle = file.SafeFileHandle;
                MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), handle, (int)type, ref info, IntPtr.Zero, IntPtr.Zero);

            }
        }
        /// <summary>
        /// Memory Dump를 생성합니다.
        /// </summary>
        /// <param name="type">메모리 덤프 타입</param>
        public static void CreateMemoryDump(MiniDumpType type = MiniDumpType.MiniDumpNormal)
        {
            var assembly = Assembly.GetEntryAssembly();
            var dirPath = Path.GetDirectoryName(assembly?.Location);
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            string dateTime = DateTime.Now.ToString("[yyyy-MM-dd][HH-mm-ss-fff]");
            var path = $"{dirPath}/[{exeName}]{dateTime}.dmp";
            CreateMemoryDump(type, path);
        }
    }
}
