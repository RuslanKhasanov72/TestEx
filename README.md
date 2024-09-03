<body>
    <div class="container">
        <p>Этот проект демонстрирует создание и запуск Docker контейнера для API. Он использует ASP.NET и MS SQL для реализации веб-API с базой данных, содержащей таблицы для участков, специализаций, кабинетов, пациентов и врачей.</p>
        <h2>Инструкция по запуску</h2>
        <h3>1. Сборка Docker образа</h3>
        <p>Для создания Docker образа выполните следующую команду в терминале:</p>
        <pre><code>docker build -t testex .</code></pre>
        <h3>2. Создание и запуск контейнера Docker</h3>
        <p>Для запуска контейнера используйте <code>docker-compose</code>. Введите команду:</p>
        <pre><code>docker-compose up</code></pre>
        <h3>3. Тестирование API</h3>
        <p>После запуска контейнера вы можете протестировать API, перейдя по следующему адресу в вашем браузере:</p>
        <p><a href="http://localhost:8080/swagger/index.html" target="_blank">http://localhost:8080/swagger/index.html</a></p>

</body>
