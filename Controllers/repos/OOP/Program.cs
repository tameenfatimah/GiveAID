using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Encapsulation | Data Hiding
            Student st = new Student();
            st.Name = "John";
            Console.WriteLine(st.Name);

            //Abstraction | Hiding Complexity
            Animal an = new Dog();
            an.MakeSound();
            
            //Inheritance | Code Reusability
            Sparrow sp = new Sparrow();
            sp.Eat();
            sp.chirp();

            //Polymorphism | Many Forms
            Family f = new Family();
            f.Compliment();
            Husband h = new Husband();
            h.Compliment();
            Wife w = new Wife();
            w.Compliment();

            //Inheritance Types
            //1. Single Inheritance
            Son s = new Son();
            s.Work();

            //2. Multilevel Inheritance
            Son1 s1 = new Son1();
            s1.Work();
            s1.study();
            Son2 s2 = new Son2();
            s2.Work();
            s2.play();

            //3. Hierarchical Inheritance
            Daughter1 d1 = new Daughter1();
            d1.Cook();
            d1.Botique();
            Daughter2 d2 = new Daughter2();
            d2.Cook();
            d2.restaurant();

            //4. Hybrid Inheritance (using interfaces) also called as multiple inheritance
            Duck duck = new Duck();
            duck.Fly();
            duck.Swim();
    
        }
    }
    //Encapsulation | Data Hiding
    class Student
    {
        private string name; 

        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    name = value;
                else
                    Console.WriteLine("Invalid name!");
            }
        }
    }

    //Abstraction | Hiding Complexity
    abstract class Animal
    {
        public abstract void MakeSound();
    }

    class Dog : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("Bark!");
        }
    }

    //Inheritance | Code Reusability
    class Bird
    {
        public void Eat()
        {
            Console.WriteLine("Eating...");
        }
    }

    class Sparrow : Bird
    {
        public void chirp()
        {
            Console.WriteLine("Chirping...");
        }
    }

    //Polymorphism | Many Forms
    class Family
    {
        public virtual void Compliment()
        {
            Console.WriteLine("A good family");
        }
    }

    class Husband : Family
    {
        public override void Compliment()
        {
            Console.WriteLine("He's a Businessman");
        }
    }

    class Wife : Family
    {
        public override void Compliment()
        {
            Console.WriteLine("She's Software Engineer");
        }
    }

    //Inheritance Types
    //1. Single Inheritance
    class Father
    {
        public void Work() => Console.WriteLine("Father works at office...");
    }

    class Son : Father
    {
        public void Study() => Console.WriteLine("Son is studying abroad...");
    }

    //2. Multilevel Inheritance
    class Parents
    {
        public void Work() => Console.WriteLine("Father works at Office...");
    }

    class Son1 : Parents
    {
        public void study() => Console.WriteLine("First son is studying...");
    }

    class Son2 : Parents
    {
        public void play() => Console.WriteLine("Second son is playing golf...");
    }

    //3. Hierarchical Inheritance
    class Mother
    {
        public void Cook() => Console.WriteLine("Mother is cooking...");
    }

    class Daughter1 : Mother
    {
        public void Botique() => Console.WriteLine("First daughter is running her botique");
    }

    class Daughter2 : Mother
    {
        public void restaurant() => Console.WriteLine("Second daughter is running her restaurant");
    }

    //4. Hybrid Inheritance (using interfaces) also called as multiple inheritance
    interface IFly
    {
        void Fly();
    }

    interface ISwim
    {
        void Swim();
    }

    class Duck : IFly, ISwim
    {
        public void Fly() => Console.WriteLine("Duck flies!");
        public void Swim() => Console.WriteLine("Duck swims!");
    }

}
