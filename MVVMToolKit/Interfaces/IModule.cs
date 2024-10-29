
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// 모듈 인터페이스
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 사용할 Type들을 등록합니다.
        /// </summary>
        /// <param name="services">서비스 컨테이너(DI)</param>
        void RegisterTypes(IServiceCollection services);
        /// <summary>
        /// 모듈을 초기화합니다.
        /// </summary>
        /// <param name="provider">서비스 제공자(DI)</param>
        /// <returns></returns>
        Task InitializeModule(IServiceProvider? provider)
        {
            return Task.CompletedTask;
        }
    }
}
