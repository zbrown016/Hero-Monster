using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Zack
{
    class Program
    {
        static void Main(string[] args)
        {
            char again = 'Y';
            do
            {
                Random dice = new Random();
                int hHealth = 15;
                int mHealth = 15;
                bool phase1 = true;
                bool phase2 = false;
                bool revive = true;
                bool death = false;
                bool charge = false;
                bool stagger = false;
                bool shield = false;
                int uses = 3;
                int action = 0;
                int iRoll;
                int cRoll;
                int half;
                string crit;

                Console.WriteLine("Welcome to Hero v Monster! Press any key to begin.");
                Console.ReadLine();

                Console.WriteLine($"\nHero encountered monster and must engage in combat!\n\nHero has {hHealth} HP and monster has {mHealth} HP.\n\nMonster can attack for 0 - 10 damage.\n\n");

                do
                {
                    do
                    {
                        do
                        {
                            try
                            {
                                Console.WriteLine($"\nWhat will hero do?\n\n1 - Attack (Deal 0 - 10 damage)\n\n2 - Heal (Recover 10 health: Potions: {uses})\n\n3 - Special (Spend a turn focusing, then deal a guaranteed 12 damage next turn)\n\n4 - Defend (Raise shield and take half damage. Also have a 25% chance to parry, taking no damage and staggering monster for next turn.\n");
                                action = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("\nInvalid format.\n");
                            }
                            if (action != 1 && action != 2 && action != 3 && action != 4)
                            {
                                Console.WriteLine("\nPlease select either 1, 2, 3, 4.\n");
                            }
                        } while (action != 1 && action != 2 && action != 3 && action != 4);

                        if (action == 1)
                        {
                            iRoll = dice.Next(0, 11);
                            mHealth -= iRoll;

                            if (mHealth <= 0)
                            {
                                mHealth = 0;
                            }

                            if (iRoll == 0)
                            {
                                Console.WriteLine("\nHero missed!\n");
                            }

                            if (iRoll >= 8)
                            {

                                crit = "Critical hit!";
                            }
                            else
                            {

                                crit = "";
                            }
                            Console.WriteLine($"\nHero dealt {iRoll} damage to monster. {crit} Monster is now at {mHealth} HP.\n");
                        }


                        if (action == 2 && uses == 0)
                        {


                            Console.WriteLine($"\nThis action cannot be done as hero has exhausted all of his healing potions.\n");


                        }
                    } while (action == 2 && uses == 0);


                    if (action == 2 & uses != 0)
                    {
                        hHealth += 10;
                        uses--;
                        Console.WriteLine($"\nHero healed for 10 health. Hero is now at {hHealth} and has {uses} potions left.\n");
                    }

                    if (action == 3)
                    {
                        if (!charge)
                        {
                            charge = true;
                            Console.WriteLine("\nHero focuses his strength for next turn.\n");
                        }
                        else
                        {
                            charge = false;
                            mHealth -= 12;
                            if (mHealth <= 0)
                            {
                                mHealth = 0;
                            }
                            Console.WriteLine($"\nHero unleashed his strength and deals 12 damage to monster. Monster now has {mHealth} health.\n");
                        }
                    }

                    if (action == 4)
                    {
                        shield = true;
                        Console.WriteLine("\nHero raised his shield.\n");
                    }

                    if (mHealth <= 0 && phase2) continue;

                    if (mHealth <= 0 && phase1)
                    {
                        phase1 = false;
                        phase2 = true;

                        Console.WriteLine("\nHero wins! Wait a minute...the monster is transforming!\n");
                        Console.WriteLine("\nThe monster is still alive and now has 25 HP.\n");
                        Console.WriteLine("\nIt also possesses a special move of its own now.\n");
                        Console.WriteLine("\nIt has a 33 % chance to attack twice in a row(4 - 7 damage per hit), so be wary!\n");
                        hHealth += 10;
                        Console.WriteLine($"\nHero healed for 10 HP during the transition. Hero now has {hHealth} HP.\n");
                        mHealth = 25;
                    }

                    if (phase1)
                    {
                        if (stagger)
                        {
                            Console.WriteLine("\nMonster is staggered and unable to attack this turn.\n");
                            stagger = false;
                        }
                        else
                        {
                            iRoll = dice.Next(0, 11);

                            if (shield)
                            {
                                shield = false;

                                if (iRoll == 0)
                                {
                                    Console.WriteLine("\nMonster missed!\n");
                                }
                                else
                                {
                                    cRoll = dice.Next(1, 5);
                                    if (cRoll == 3)
                                    {
                                        stagger = true;
                                        Console.WriteLine("\nHero successfully parried the monster's attack! Hero sustains no damage and the monster cannot attack next turn.\n");

                                    }
                                    else
                                    {
                                        half = (iRoll / 2);
                                        hHealth -= half;
                                        if (hHealth <= 0)
                                        {
                                            hHealth = 0;
                                        }
                                        Console.WriteLine($"\nHero defended against the monster's blow and took {half} damage. Hero is now at {hHealth}.\n");

                                    }
                                }

                            }
                            else
                            {
                                hHealth -= iRoll;
                                if (iRoll == 0)
                                {
                                    Console.WriteLine("\nMonster missed!\n");
                                }
                                if (iRoll >= 8)
                                {
                                    crit = "Critical hit!";
                                }
                                else
                                {
                                    crit = "";
                                }
                                if (hHealth <= 0)
                                {
                                    hHealth = 0;
                                }
                                Console.WriteLine($"\nMonster dealt {iRoll} damage to hero. {crit} Hero is now at {hHealth} HP.\n");
                            }

                        }

                    }
                    if (phase2)
                    {
                        if (stagger)
                        {
                            Console.WriteLine("\nMonster is staggered and unable to attack this turn.\n");
                            stagger = false;
                        }
                        else
                        {
                            cRoll = dice.Next(1, 4);
                            if (cRoll == 3)
                            {
                                iRoll = dice.Next(4, 8);
                                int firsthit = iRoll;
                                iRoll = dice.Next(4, 8);
                                int secondhit = iRoll;
                                int sum = firsthit + secondhit;
                                if (shield)
                                {
                                    shield = false;
                                    cRoll = dice.Next(1, 5);
                                    if (cRoll == 3)
                                    {
                                        stagger = true;
                                        Console.WriteLine("\nHero successfully parried the monster's attack! Hero sustains no damage and the monster cannot attack next turn.\n");

                                    }
                                    else
                                    {
                                        half = sum / 2;
                                        hHealth -= half;
                                        if (hHealth <= 0)
                                        {
                                            hHealth = 0;
                                        }
                                        Console.WriteLine($"\nHero defended against the monster's special attack and took {half} damage. Hero is now at {hHealth}.\n");
                                    }

                                }
                                else
                                {
                                    hHealth -= sum;
                                    if (hHealth <= 0)
                                    {
                                        hHealth = 0;
                                    }
                                    Console.WriteLine($"\nMonster used its special and swiped twice! Once for {firsthit} damage and another for {secondhit} damage! Hero is now at {hHealth}.\n");
                                }

                            }
                            else
                            {
                                iRoll = dice.Next(0, 11);

                                if (shield)
                                {
                                    shield = false;

                                    if (iRoll == 0)
                                    {
                                        Console.WriteLine("\nMonster missed!\n");
                                    }
                                    else
                                    {
                                        cRoll = dice.Next(1, 5);
                                        if (cRoll == 3)
                                        {
                                            stagger = true;
                                            Console.WriteLine("\nHero successfully parried the monster's attack! Hero sustains no damage and the monster cannot attack next turn.\n");

                                        }
                                        else
                                        {
                                            half = iRoll / 2;
                                            hHealth -= half;
                                            if (hHealth <= 0)
                                            {
                                                hHealth = 0;
                                            }
                                            Console.WriteLine($"\nHero defended against blow and took {half} damage. Hero is now at {hHealth}.\n");

                                        }
                                    }

                                }
                                else
                                {
                                    hHealth -= iRoll;
                                    if (iRoll == 0)
                                    {
                                        Console.WriteLine("\nMonster missed!\n");
                                    }
                                    if (iRoll >= 8)
                                    {
                                        crit = "Critical hit!";
                                    }
                                    else
                                    {
                                        crit = "";
                                    }
                                    if (hHealth <= 0)
                                    {
                                        hHealth = 0;
                                    }
                                    Console.WriteLine($"\nMonster dealt {iRoll} damage to hero. {crit} Hero is now at {hHealth} HP.\n");
                                }

                            }
                        }

                    }

                    if (hHealth <= 0 && revive)
                    {
                        hHealth = 5;
                        Console.WriteLine($"\nHero was holding an auto-revive which resurrects him and puts his HP at 5. He's not done yet!\n\n");
                        revive = false;
                        death = true;

                    }
                } while (hHealth > 0 && mHealth > 0);



                if (mHealth <= 0 && phase2)
                {
                    Console.WriteLine("\nHero slayed the monster!\n");

                    do
                    {
                        Console.WriteLine("\nPlay again?\n\nY - Yes\n\nN - No\n");
                        again = char.ToUpper(Convert.ToChar(Console.ReadLine()));


                        switch (again)
                        {
                            case 'Y':

                                Console.Clear();
                                Console.WriteLine("\x1b[3J");
                                break;

                            case 'N':

                                Console.WriteLine("\nGame over.\n");
                                break;

                            default:

                                Console.WriteLine("\nInvalid input. You can only answer Y(Yes) or N(No).\n");
                                break;


                        }
                    } while (again != 'Y' && again != 'N');
                }

                if (hHealth <= 0 && death)
                {

                    Console.WriteLine("\nHero died!\n");

                    do
                    {


                        Console.WriteLine("\nPlay again?\n\nY - Yes\n\nN - No\n");
                        again = char.ToUpper(Convert.ToChar(Console.ReadLine()));

                        switch (again)
                        {
                            case 'Y':

                                Console.Clear();
                                Console.WriteLine("\x1b[3J");
                                break;

                            case 'N':

                                Console.WriteLine("\nGame over.\n");
                                break;

                            default:

                                Console.WriteLine("\nInvalid input. You can only answer Y(Yes) or N(No.\n");
                                break;


                        }
                    } while (again != 'Y' && again != 'N');
                }
            } while (again == 'Y');
        }
    }
}













