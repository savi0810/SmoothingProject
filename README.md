# SmoothingProject

## Обзор
**SmoothingProject** — приложение на WPF, демонстрирующее гауссово сглаживание численных данных и визуализацию результата. Проект нацелен на .NET 9.0 и использует Windows Presentation Foundation для отображения интерфейса. Основная логика реализована в нескольких сервисных классах, а главное окно связывает их в коде облицовки (code‑behind).

Репозиторий включает набор тестов NUnit, покрывающих сервисы и логику окна. 

## Возможности
- **Генерация случайных данных** – эмуляция шумных измерений через `DataService`.
- **Применение гауссова сглаживания** – фильтрация последовательности в `GaussianSmoother`.
- **Визуализация** – отображение исходных и сглаженных значений на двух элементах `Canvas` с помощью `GraphPlotter`.
- **Обработка ошибок** – аргументы валидируются, исключения показываются в виде всплывающих окон.

## Структура проекта
Решение содержит два проекта:

### `SmoothingProject` (приложение)
- `App.xaml` / `App.xaml.cs` – точка входа, указывает `StartupUri`.
- `MainWindow.xaml` / `MainWindow.xaml.cs` – главное окно; кнопки запускают генерацию данных и сглаживание, а канвасы отображают результаты. Вся логика представления находится в code‑behind.
- `Services/`
  - `DataService.cs` – создает списки случайных `double`.
  - `GaussianSmoother.cs` – выполняет гауссово взвешенное сглаживание.
  - `GraphPlotter.cs` – рисует линии и метки на `Canvas`.

Явных каталогов `ViewModels` или `Models` нет; для текущего объема хватает code‑behind и сервисов.

### `SmooshingProject.Tests` (модульные тесты)
Содержит классы NUnit, тестирующие каждый сервис и поведение окна (например, генерацию данных и очистку канвасов). Тесты находятся в отдельном проекте.

## Быстрый старт
1. **Клонирование репозитория**
   ```sh
   git clone <repository-url>
   cd SmoothingProject
   ```
2. **Установка SDK**
   Убедитесь, что на Windows установлен .NET 9.0 SDK с поддержкой WPF.
3. **Восстановление пакетов**
   В решении нет внешних пакетов NuGet; выполните **`dotnet restore`**, если добавите какие‑либо позже.
4. **Сборка**
   ```sh
   dotnet build SmoothingProject.sln
   ```
   или откройте решение в Visual Studio и соберите там.
5. **Запуск**
   ```sh
   dotnet run --project SmoothingProject/SmoothingProject.csproj
   ```
   Также можно нажать F5 в Visual Studio.

## Примеры кода
Фрагменты ниже показывают, как используются компоненты в коде.

**Инициализация сервисов в конструкторе окна:**
```csharp
public MainWindow()
{
    InitializeComponent();
    dataService = new DataService();
    smoother = new GaussianSmoother();
    plotter = new GraphPlotter();
    Loaded += (s, e) => { GenerateData(); PlotOriginalData(); };
}
```

**Генерация и сглаживание данных:**
```csharp
public void GenerateData() =>
    originalData = dataService.GenerateSampleData(1000);

public void ApplySmoothing() =>
    smoothedData = smoother.Smooth(originalData, 66, 10);
```

**Построение на canvas:**
```csharp
private void PlotOriginalData()
{
    if (LeftCanvas.ActualWidth > 0 && LeftCanvas.ActualHeight > 0)
    {
        plotter.PlotPoints(LeftCanvas, originalData, false);
    }
}
```


## Технологии
- **Язык:** C# (включены nullable-ссылки)
- **Платформа:** .NET 9.0, WPF
- **Паттерн:** простое code-behind, без MVVM‑фреймворка
- **Тестирование:** NUnit
- **NuGet-пакеты:** отсутствуют (только библиотеки .NET SDK)

---

