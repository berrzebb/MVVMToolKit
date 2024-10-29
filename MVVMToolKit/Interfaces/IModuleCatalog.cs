namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 응용프로그램에서 사용하는 Module들의 목록을 관리하는 객체.
    /// </summary>
    public interface IModuleCatalog : IEnumerable<IModule>
    {
        /// <summary>
        /// 모듈을 등록합니다.
        /// </summary>
        /// <param name="module">등록할 모듈 인터페이스</param>
        /// <returns>모듈 카탈로그 객체</returns>
        IModuleCatalog AddModule(IModule? module);
        /// <summary>
        /// 모듈을 등록합니다.
        /// </summary>
        /// <param name="moduleType">등록할 모듈의 타입</param>
        /// <returns>모듈 카탈로그</returns>
        IModuleCatalog AddModule(Type moduleType);
        /// <summary>
        /// 모듈을 등록합니다.
        /// </summary>
        /// <typeparam name="TModule">등록할 모듈의 타입</typeparam>
        /// <returns>모듈 카탈로그</returns>
        IModuleCatalog AddModule<TModule>() where TModule : IModule;
    }
}
