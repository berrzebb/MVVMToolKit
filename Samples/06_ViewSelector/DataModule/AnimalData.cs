using DataModule.Model;
using System;
using System.Collections.Generic;
using MVVMToolKit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModule
{
    public class AnimalData : IAnimalData
    {
        public Animal[] Animals { get; } = new Animal[]
        {
            new Animal(1, "나무구멍 또는 땅굴에서 단독으로 동면한다.\r\n\r\n영역을 가지고 방어하며, 나무를 매우 잘 타지만 땅 위에서 더 많이 활동한다. 나무 구멍을 이용하기도 하나 대부분 땅 위에 굴을 파서 번식이나 동면을 한다. 주행성이며 겨울철에 동면에 들어간다. 동면에 들어가기 전에는 먹이를 저장하는 습성이 있다. 봄에 교미하고 여름철에 3~7마리의 새끼를 낳는다. 수유기간은 50~57일이며 성성숙은 약 1세에 이루어진다.\r\n[네이버 지식백과] 다람쥐 [Siberian Chipmunk]", "다람쥐"),
            new Animal(2,
                "학명은 Paradoxornis webbiana fulvicauda (CAMPBELL)이다. 붉은머리오목눈이라고도 하며, 북한에서는 부비새 또는 비비새라고 부른다. 동부 아시아에 분포하고 있으며 우리 나라에서는 전역에서 흔히 번식하는 텃새이다.\r\n\r\n등은 적갈색이고 배는 황갈색이다. 수컷의 등은 핑크색을 띄나 암컷은 연한 색깔이다. 재빠른 동작으로 움직일 때 긴 꼬리를 좌우로 흔든다. 번식기 이외에는 대개 30\u223c50마리의 무리가 쉬지 않고 ‘씨씨씨씨’ 울면서 관목 속을 재빨리 움직이는 것을 볼 수 있다.\r\n\r\n관목지대와 덤불 또는 농경지와 풀밭 등 평지와 구릉에서 번식을 하는데 4\u223c7월에 한배에 4, 5개의 알을 낳는다. 주식물은 풀씨와 곤충류이다. 굴뚝새와 비슷하여 혼동되기도 한다.\r\n[네이버 지식백과]",
                new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DataModule;component/Images/Bird.png")),
                "뱁새" )
        };
    }
}
