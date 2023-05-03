using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace JJJ16
{

    class Score : IComparable<Score>
    {
        public int number;
        public Score(int number)
        {
            this.number = number;
        }

        public int CompareTo(Score? other)
        {
            return number.CompareTo(other.number);
        }

        public override string ToString()
        {
            return number.ToString();
        }

        // 연산자 오버로딩
        // 접근 제한자 + static + 반환형 + operator + 연산자 + (비교값a, b)

        public static bool operator <(Score left, Score right)
        {
            return left.number < right.number;
        }
        public static bool operator >(Score left, Score right)
        {
            return !(left < right);
        }
        public static bool operator ==(Score left, Score right)
        {
            return left.number.CompareTo(right.number) == 0;
        }
        public static bool operator != (Score left, Score right)
        {
            return !(left == right);    
        }
    }

    public static class Alignment
    {
        public delegate int Comparer<T>(T t1, T t2);
        public static void Sort<T>(T[] array, bool isAscending = true)
            where T : IComparable<T>
        {
            //                                 ? (true)   :   (false)
            Comparer<T> compare = isAscending ? Ascending : Descending;

            // 버블정렬 (Bubble sort)
            int length = array.Length - 1;
            for (int index = 0; index < length; index++)
            {
                for (int i = 0; i < length - index; i++)
                {
                    if (compare(array[i], array[i + 1]) == 1)
                        Swap(array, i, i + 1);
                }
            }

        }
        static void Swap<T>(T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
        // 오름차순.
        static int Ascending<T>(T a, T b)
            where T : IComparable<T>
        {
            return a.CompareTo(b);
        }
        // 내림차순
        static int Descending<T>(T a, T b)
            where T : IComparable<T>
        {
            return a.CompareTo(b) * -1;
        }

    }
    internal class Program
    {
        // 델리게이트 : 함수를 참조할 수 있는 자료형
        // 형식이 같기만 하면 누구든 참조 가능
        delegate string ConvertString(int a, int b);

        static string SumToString(int num1,  int num2)
        {
            return (num1 + num2).ToString();
        
        }

        class GameServer
        {
            // 델리게이트 : 함수를 참조할 수 있는 자료형
            // 형식이 같기만 하면 누구든 참조 가능
            // 아래는 델리게이트를 이용하여 다른 플렛폼에서 다른 함수명을 가진 함수들을 일괄적으로 처리하는 방법
            // 특정 객체를 알고있을 필요가 사라짐

            public delegate void SendAD(string msg);
            SendAD sendAD; // 함수 포인터 +, - 로 함수 체인이 가능하다. 호출 시, 체인 되어 있는 함수들이 차례대로 호출된다.

            public void Regested(SendAD func = null)
            {
                if (func != null)
                    sendAD += func;
            }
            public void Release(SendAD func)
            {
                sendAD -= func;
            }

            public void SendMessage(string msg)
            {
                sendAD(msg);
            }
        }

        class MoblieDevice
        {
            string nickname;
            public MoblieDevice(string nickname)
            {
                this.nickname = nickname;
            }
            public void AgreeToReceive(GameServer server)
            {
                server.Regested(OnReciveAD);
            }

            public void ReleaseToreceive(GameServer server)
            {
                server.Release(OnReciveAD);
            }


            private void OnReciveAD(string msg)
            {
                Console.WriteLine($"{nickname} {msg}");
            }
        }

        class Computer
        {
            string userName;
            public Computer(string userName)
            {
                this.userName = userName;
            }
            public void AgreeAD(GameServer server)
            {
                server.Regested(OnShowAD);
            }
            public void DisagreeAD(GameServer server)
            {
                server.Release(OnShowAD);
            }
            private void OnShowAD(string msg)
            {
                Console.WriteLine($"{userName}{msg}");
            }
        }

        // 기본 정의 델리게이트
        // -> 별도의 정의 없이 손쉽게 델리게이트를 만드는 방법
        // T값을 받는 함수를 대입할 경우 기본 형식으로 제작한 델리게이트로는 불가능하다. (기본 델리게이트 제작 함수가 T로 값을 받기 때문)
        // 이 경우엔 정식 포맷으로 델리게이트를 작성해주어야한다.

        // 반환형이 없고 double을 하나 받는 함수를 대입하는 델리게이트
        
        delegate void Question(double d);
        delegate void PrintString(string s);
        // statioc void Question(double d) 같은 이름의 함수 못씀
        static void PrintS(string s)
        {

        }

        static void Main(string[] args)
        {
            // 델리게이트 예시들
            #region
            // 델리게이트 변수에 형식에 맞는 함수를 대입하고 있다.
            // 1. int형 string으로 바꾸는 함수 만들기
            ConvertString conversion = SumToString;
            string str = conversion(10, 20);
            Console.WriteLine(str);

            // 2. 게임 서버에서 일괄적으로 플랫폼에 메세지 보내기
            GameServer game1 = new GameServer();
            MoblieDevice m1 = new MoblieDevice("유저A");
            MoblieDevice m2 = new MoblieDevice("유저B");
            MoblieDevice m3 = new MoblieDevice("유저C");
            MoblieDevice m4 = new MoblieDevice("유저D");

            m1.AgreeToReceive(game1);
            m2.AgreeToReceive(game1);
            m4.AgreeToReceive(game1);

            game1.SendMessage("팡고1");

            m1.ReleaseToreceive(game1);

            game1.SendMessage("새해맞이 특별 이벤트");

            Computer pc1 = new Computer("PC-A");
            Computer pc2 = new Computer("PC-b");
            Computer pc3 = new Computer("PC-c");

            pc1.AgreeAD(game1);

            game1.SendMessage("새해맞이 특별 이벤트2");
            #endregion
            // 거품 정렬 실습
            #region
            Score score1 = new Score(10);
            Score score2 = new Score(20);

            Console.WriteLine(score1 < score2); // 연산자 오버로딩 없이는 비교 불가능

            // CompareTo : int <IComparable>(비교 함수)
            // L.value와 R.value를 비교하여 L이 더 작으면 -1, 크면 1을 반환한다. (같으면 0)


            int num1 = 10;
            Console.WriteLine(num1.CompareTo(20)); // int 값만 비교 가능

            Score[] scores = new Score[] { new Score(5), new Score(4), new Score(2) };
            
            Alignment.Sort(scores, false);

            Console.WriteLine(scores[0]);
            Console.WriteLine(scores[1]);
            Console.WriteLine(scores[2]);
            #endregion

            // 기본 정의 델리게이트 (Action, Func, Invoke)
            // Action : 반환형이 없는 기본 정의 델리게이트
            System.Action<double> onQuestion;
            System.Action<string> PrintString;

            Question onQuest; // 같은 의미

            // 반환형이 없고 매개변수로 double을 하나 받는 기본 델리게이트
            Action<string> onPrint = PrintS;
            Action<string> onPrint3 = null;

            if (onPrint3 != null)
            {
                onPrint3("ABC");
            }
            // nullable형식 string 변수
            string? nullString = null;
            Console.WriteLine((nullString == null) ? "Null입니다." :  nullString);

            // nullable형식 변수 ?? A
            // -> 값이 null이면 A를 사용하고 아니면 변수를 사용한다.
            Console.WriteLine(nullString ?? "Null입니다.");

            // Invoke는 함수를 호출하겠다 라는 명령어
            // -> ? 해당값이 null이라면 무시하고 아니라면 내부에 접근하겠다.
            Action<string> onPrint4 = null;
            onPrint4?.Invoke("ABC");

            // Func : 반환형이 있는 기본 델리게이트 작성
            // 각괄호 안 마지막에 들어가는 자료형은 반환형이다.

            Func<int, int, string> onPrint5 = null; // 반환형이 있고 매개변수로 int를 2개 받는 기본 델리게이트
            Func<string> onPrint6 = null; //string을 반환하고 매개변수가 없는 델리게이트
            Func<int,double, string> onPrint7 = null; // string을 반환하고 int, double을 매개변수로 받는 델리게이트

            // 반환형도 매개변수도 없는 기본 델리게이트 작성
            Action onPrint0 = null;
        }
    }
}