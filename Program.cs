using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spartadungeon
{
    class Program
    {
        static Player player = new Player();

        static void Main(string[] args)
        {
            ShowStartMenu();
        }

        static void ShowStartMenu()
        {
            Console.WriteLine("=== Spartadungeon ===");
            Console.WriteLine("1. 게임 시작");
            Console.WriteLine("2. 종료");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ShowIntro();
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    ShowStartMenu();
                    break;
            }
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("당신은 위대한 영웅들의 고향 스파르타 마을에 도착했습니다...");
            Console.WriteLine("위대한 영웅들 처럼 모험을 시작하려면 캐릭터를 생성하세요!");
            CreateCharacter();
        }

        static void CreateCharacter()
        {
            Console.Write("캐릭터 이름을 입력하세요: ");
            player.Name = Console.ReadLine();

            Console.Write("직업을 입력하세요 (예: 전사 / 마법사): ");
            player.Job = Console.ReadLine();

            player.Level = 1;
            player.HP = 100;
            player.Attack = 10;
            player.Defense = 5;
            player.Gold = 100;
            player.Inventory = new List<Item>(); // 초기 인벤토리 비움
            player.EquippedItem = null;

            Console.WriteLine($"환영합니다, {player.Name}님! 직업: {player.Job}");
            ShowMainMenu();
        }


        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

                Console.WriteLine("\n=== 메인 메뉴 ===");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 종료");
                Console.Write("선택: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.ShowStatus();
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        Store.EnterStore(player);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }
    }

    class Player
    {
        public int Level = 1;
        public string Name;
        public string Job;
        public int Attack;
        public int Defense;
        public int HP;
        public int Gold;

        public List<Item> Inventory = new List<Item>();
        public Item EquippedItem = null;

        public void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            int totalAttack = Attack + (EquippedItem?.AttackBonus ?? 0);
            int totalDefense = Defense + (EquippedItem?.DefenseBonus ?? 0);

            Console.WriteLine($"Lv .{Level:D2}");
            Console.WriteLine($"{Name} ({Job})");
            Console.WriteLine($"공격력 : {totalAttack}");
            Console.WriteLine($"방어력 : {totalDefense}");
            Console.WriteLine($"체 력 : {HP}");
            Console.WriteLine($"Gold : {Gold}G\n");

            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                return;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowStatus(); // 다시 표시
            }
        }

        public void ShowInventory()
        {
            Console.WriteLine("\n=== 인벤토리 ===");

            if (Inventory.Count == 0)
            {
                Console.WriteLine("아이템이 없습니다.");
                return;
            }

            for (int i = 0; i < Inventory.Count; i++)
            {
                Item item = Inventory[i];
                string displayName = (item == EquippedItem) ? $"[E] {item.Name}" : item.Name;
                Console.WriteLine($"{i + 1}. {displayName}");
            }

            Console.Write("\n장착할 아이템 번호를 입력하세요 (0: 나가기): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= Inventory.Count)
            {
                EquippedItem = Inventory[index - 1];
                Console.WriteLine($"{EquippedItem.Name}을(를) 장착했습니다!");
            }
            else if (index != 0)
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }

        public void ManageEquipment()
        {
            Console.WriteLine("\n장착할 아이템을 선택하세요:");
            ShowInventory();

            Console.Write("선택 (번호): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index <= Inventory.Count)
            {
                EquippedItem = Inventory[index - 1];
                Console.WriteLine($"{EquippedItem.Name}을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }
    }

    class Item
    {
        public string Name;
        public int Price;
        public int AttackBonus;
        public int DefenseBonus;

        public Item(string name, int price, int attackBonus = 0, int defenseBonus = 0)
        {
            Name = name;
            Price = price;
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
        }
    }

    class Store
    {
        public static void EnterStore(Player player)
        {
            List<Item> storeItems = new List<Item>
            {
                new Item("나무 검", 100, attackBonus: 5),
                new Item("철 방패", 120, defenseBonus: 7)
            };
            
            Console.WriteLine("\n=== 상점 ===");
            for (int i = 0; i < storeItems.Count; i++)
                Console.WriteLine($"{i + 1}. {storeItems[i].Name} - {storeItems[i].Price} Gold");

            Console.Write("구매할 아이템 번호 입력 (0: 취소): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= storeItems.Count)
            {
                Item selectedItem = storeItems[choice - 1];
                if (player.Gold >= selectedItem.Price)
                {
                    player.Gold -= selectedItem.Price;
                    player.Inventory.Add(selectedItem);
                    Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다!");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다!");
                }
            }
        }
    }
}
