using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab_9.Commands;
using Lab_9.Models;
namespace Lab_9.ViewModels{
    public class MainViewModel : INotifyPropertyChanged {
        private readonly BinarySearchTree tree = new();
        private string input = "";
        private string status = "Дерево порожнє.";
        private string comparison = "Порівняння ще не виконано.";
        public Node? Root => tree.Root;
        public string Input{
            get => input;
            set {
                input = value;
                OnPropertyChanged();
            }
        }
        public string Status{
            get => status;
            set {
                status = value;
                OnPropertyChanged();
            }
        }
        public string Comparison{
            get => comparison;
            set {
                comparison = value;
                OnPropertyChanged();
            }
        }
        public int Count => tree.Count;
        public int Height => tree.Height;
        public string Min => tree.Min?.ToString() ?? "Немає даних";
        public string Max => tree.Max?.ToString() ?? "Немає даних";
        public List<int> Preorder => tree.Preorder();
        public List<int> Inorder => tree.Inorder();
        public List<int> Postorder => tree.Postorder();
        public ICommand AddCommand { get; }
        public ICommand FindCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DemoCommand { get; }
        public ICommand CompareCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? TreeChanged;
        public MainViewModel(){
            AddCommand = new RelayCommand(Add);
            FindCommand = new RelayCommand(Find);
            DeleteCommand = new RelayCommand(Delete);
            ClearCommand = new RelayCommand(Clear);
            DemoCommand = new RelayCommand(LoadDemo);
            CompareCommand = new RelayCommand(CompareTrees);
        }
        private void Add() {
            string[] values = Input.Split( new[] { ' ', ',' },StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0) {
                Status = "Введіть інвентарні номери.";
                return;
            }
            string message = "";
            foreach (string item in values){
                if (!int.TryParse(item, out int number) || number < 1 || number > 999999){
                    message += $"{item}: некоректний номер.\n";
                }
                else if (tree.Add(number)) {
                    message += $"{number}: додано.\n";
                }
                else{
                    message += $"{number}: номер уже існує.\n";
                }
            }
            Status = message.TrimEnd();
            Input = "";
            Refresh();
        }
        private bool ReadNumber(out int number){
            if (!int.TryParse(Input, out number) || number < 1 || number > 999999){
                Status = "Введіть один цілий номер від 1 до 999999.";
                return false;
            }
            return true;
        }
        private void Find(){
            if (!ReadNumber(out int number))
                return;
            bool found = tree.Find(number, out string path);
            if (string.IsNullOrEmpty(path))
                path = "Дерево порожнє";
            Status = found
                ? $"Номер {number} знайдено.\nШлях: {path}"
                : $"Номер {number} не знайдено.\nШлях: {path}";
        }
        private void Delete(){
            if (!ReadNumber(out int number))
                return;
            if (tree.Delete(number)){
                Status = $"Номер {number} видалено.";
                Input = "";
                Refresh();
            }
            else {
                Status = $"Номер {number} відсутній у дереві.";
            }
        }
        private void Clear(){
            tree.Clear();
            Input = "";
            Status = "Дерево очищено.";
            Refresh();
        }
        private void LoadDemo(){
            tree.Clear();
            int[] numbers = { 50, 30, 70, 20, 40, 60, 80 };
            foreach (int number in numbers)
                tree.Add(number);
            Input = "";
            Status = "Демонстраційний набір завантажено.";
            Refresh();
        }
        private void CompareTrees() {
            const int size = 100;
            int[] numbers = Enumerable.Range(1, size).ToArray();
            BinarySearchTree sortedTree = new();
            BinarySearchTree randomTree = new();
            foreach (int number in numbers)
                sortedTree.Add(number);
            Random random = new();
            for (int i = numbers.Length - 1; i > 0; i--)  {
                int j = random.Next(i + 1);
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
            }
            foreach (int number in numbers)
                randomTree.Add(number);
            Comparison =
                $"Кількість вузлів: {size}\n" +
                $"Випадкова послідовність: висота {randomTree.Height}\n" +
                $"Відсортована послідовність: висота {sortedTree.Height}";
            int count = 0;
            foreach (int number in randomTree)
                count++;
            Comparison += $"\nПеребір через foreach: {count} елементів.";
        }
        private void Refresh() {
            OnPropertyChanged(nameof(Root));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(Min));
            OnPropertyChanged(nameof(Max));
            OnPropertyChanged(nameof(Preorder));
            OnPropertyChanged(nameof(Inorder));
            OnPropertyChanged(nameof(Postorder));
            TreeChanged?.Invoke();
        }
        private void OnPropertyChanged( [CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs(propertyName));
        }
    }
}