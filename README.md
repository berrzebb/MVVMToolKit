Microsoft .Net Hosting 환경에서 MVVM을 사용하고, MVVM의 달성을 위해 방해되는 요소들을 만들어둔 MVVM 라이브러리입니다.

내부 사용 환경은 다음과 같습니다.
 - Microsoft.Extensins.Hosting
 - Microsoft.Xaml.Behaviors.Wpf
 - Community Toolkit

기본적으로 DI로 제공 되는 서비스는 다음과 같습니다.
 - IDispatcherService(UI Thread)
 - IDialogService(Popup을 띄우기 위한 서비스입니다.)
 - IZoneNavigator(View의 영역과 전환을 관리하는 기능을 가집니다.)
 - IDisposableObjectService(관리가 필요한 객체들을 정리하기 위한 글로벌 객체 해제 서비스입니다.)

