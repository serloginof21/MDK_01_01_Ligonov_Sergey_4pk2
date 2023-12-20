using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reactive.Subjects;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PZ_14
{
    private List<TextBox> gradeTextBoxes;  // Список текстовых полей для оценок
    private TextBox ballTextBox;          // Текстовое поле для средней оценки
    private Subject<IEnumerable<double>> gradesSubject = new Subject<IEnumerable<double>>();  // Подписчик на изменения оценок
    public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();  // Коллекция студентов

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация списка текстовых полей для оценок
            gradeTextBoxes = new List<TextBox>
            {
                FirGread, TwoGread, ThirGread, FourGread, FiveGread, SixGread, SevenGread
            };

            // Инициализация текстового поля для средней оценки
            ballTextBox = Ball;

            // Создание потока для изменений оценок
            var gradeChanges = gradeTextBoxes
                .Select(textBox => Observable.FromEventPattern<TextChangedEventArgs>(textBox, "TextChanged")
                    .Select(_ => ParseGrade(textBox.Text)));


            // Объединение изменений в один поток
            var combinedChanges = Observable.CombineLatest(gradeChanges.ToArray());



            // Подписка на изменения оценок
            combinedChanges.Subscribe(grades => gradesSubject.OnNext(grades));

            // Подписка на изменения оценок для обновления статистики
            gradesSubject.Subscribe(UpdateStatistics);

            // Добавление текстовых полей для первого студента
            AddTextBoxesForStudent(Students.FirstOrDefault());
        }

        // Обработчик изменения текста в поле оценки
        private void GradeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {   // Получение объект TextBox, который вызвал событие
            TextBox textBox = (TextBox)sender;
            // Проверяем, не пусто ли текстовое поле
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                // Пытаемся преобразовать текст в число
                if (double.TryParse(textBox.Text, out double value))
                {
                    if (value < 2 || value > 5)
                    {
                        MessageBox.Show("Введите число от 2 до 5.");
                        // Очистка текстового поля или другие действия по вашему усмотрению
                        textBox.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректное число.");
                    // Очистка текстового поля или другие действия по вашему усмотрению
                    textBox.Text = "";
                }
                // Оповещаем подписчиков о изменении оценок
                gradesSubject.OnNext(gradeTextBoxes.Select(tb => ParseGrade(tb.Text)));
            }
        }

        // Обновление статистики
        private void UpdateStatistics(IEnumerable<double> grades)
        {
            // Проверка значений асинхронно
            UpdateGrades(grades);
            UpdateStatistics1();
        }


        // Обновление статистики для отображения средней оценки
        private void UpdateStatistics1()
        {

            Ball.Text = $"{AverageGrade:F2}";
        }

        // Преобразование строки в число
        private double ParseGrade(string grade)
        {
            try
            {
                return string.IsNullOrEmpty(grade) ? 0 : Convert.ToDouble(grade);
            }
            catch
            {
                MessageBox.Show("Без нулей");
                return 0;
            }
        }

        // Список оценок студента
        private List<double> _grades = new List<double>();

        // Количество студентов
        public int StudentCount => _grades.Count;

        // Средняя оценка
        public double AverageGrade => _grades.Any() ? _grades.Average() : 0;

        // Обновление списка оценок
        private void UpdateGrades(IEnumerable<double> grades)
        {
            _grades.Clear();
            _grades.AddRange(grades);
        }

        // Добавление нового студента
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var newStudent = new Student
            {
                Name = "",
                Grade1 = "",
                Grade2 = "",
                Grade3 = "",
                Grade4 = "",
                Grade5 = "",
                Grade6 = "",
                Grade7 = ""
            };

            Students.Add(newStudent);
            AddTextBoxesForStudent(newStudent);
            UpdateStatistics1();
        }

        // Добавление текстовых полей для студента
        private void AddTextBoxesForStudent(Student student)
        {
            if (student == null)
            {
                MessageBox.Show("Ошибка: объект Student равен null");
                return;
            }

            // Текстовое поле для имени
            var nameTextBox = new TextBox
            {
                Text = student.Name,
                FontSize = 12,
                TextAlignment = TextAlignment.Center
            };
            Grid.SetRow(nameTextBox, Students.IndexOf(student) + 2);
            Grid.SetColumn(nameTextBox, 0);
            myGrid.Children.Add(nameTextBox);

            // Список текстовых полей для оценок
            var textBoxes = new List<TextBox>
            {
                new TextBox { Text = student.Grade1, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade2, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade3, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade4, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade5, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade6, FontSize = 16, TextAlignment = TextAlignment.Center },
                new TextBox { Text = student.Grade7, FontSize = 16, TextAlignment = TextAlignment.Center }
            };

            int rowIndex = Students.IndexOf(student) + 2;
            int columnIndex = 1;

            // Добавление текстовых полей для оценок в сетку
            foreach (var textBox in textBoxes)
            {
                Grid.SetRow(textBox, rowIndex);
                Grid.SetColumn(textBox, columnIndex++);
                gradeTextBoxes.Add(textBox);
                myGrid.Children.Add(textBox);
            }
        }
    }
}
