window.addEventListener('load', function(){
    const canvas = document.getElementById('canvas1');
    const ctx = canvas.getContext("2d");

    canvas.width = 1400;
    canvas.height = 720;

    // Multiple Enemies
    let enemies = [];
    let score = 0;
    let gameOver = false;

    const fullScreenButton = document.getElementById('fullScreenButton');

    class InputHandler {
        constructor(){
            // Track Multiple Inputs (key)

            // lexical scoping
            // Key Events
            this.keys = [];
            this.touchY = '';

            // limit
            this.touchThreshold = 30;

            window.addEventListener('keydown', e => {
                if((    e.key === 'ArrowDown' || 
                        e.key === 'ArrowUp' ||
                        e.key === 'ArrowLeft' ||
                        e.key === 'ArrowRight') 
                        && this.keys.indexOf(e.key) === -1){
                    this.keys.push(e.key);
                } else if(e.key === 'Enter' && gameOver) restartGame();
            });

            window.addEventListener('keyup', e => {
                if(    e.key === 'ArrowDown' || 
                        e.key === 'ArrowUp' ||
                        e.key === 'ArrowLeft' ||
                        e.key === 'ArrowRight'){
                    // splice : remove element
                    this.keys.splice(this.keys.indexOf(e.key), 1);
                }
            });
            window.addEventListener('touchstart', e => {
                this.touchY = e.changedTouches[0].pageY;

            });
            window.addEventListener('touchmove', e => {
                const swipeDistance = e.changedTouches[0].pageY - this.touchY;
                if(swipeDistance < -this.touchThreshold && this.keys.indexOf('swipe up') === -1) this.keys.push('swipe up');
                else if(swipeDistance > this.touchThreshold && this.keys.indexOf('swipe down') === -1) {
                    this.keys.push('swipe down');
                    if(gameOver) restartGame();
                }
            });
            window.addEventListener('touchend', e => {
                this.keys.splice(this.keys.indexOf('swipe up'), 1);
                this.keys.splice(this.keys.indexOf('swipe down'), 1);
            });

        }
    }

    class Player{
        constructor(gameWidth, gameHeight){
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
            this.width = 200;
            this.height = 200;
            this.x = 0;
            this.y = this.gameHeight - this.height;
            this.image = document.getElementById('playerImage');

            // Can Change Resource(player) with Changing :
            this.frameX = 0;
            this.frameY = 1;

            // 8 horizontal frame for Player
            this.maxFrame = 8;

            this.fps = 20;
            this.frameTimer = 0;
            this.frameInterval = 1000/this.fps;

            // Positive : move Right | Negative : move Left
            this.speed = 0;

            // Jump
            this.vy = 0;
            this.weight = 1;
        }
        restart(){
            this.x = 100;
            this.y = this.gameHeight - this.height;

            this.maxFrame = 8;
            this.frameY = 0;
        }
        draw(context){
            context.drawImage(this.image, this.frameX * this.width, this.frameY * this.height, this.width, this.height, this.x, this.y, this.width, this.height);
        }
        update(input, deltaTime, enemies){

            // collision detection
            enemies.forEach(enemy =>{
                const dx = (enemy.x + enemy.width/2 - 20) - (this.x + this.width/2);
                const dy = (enemy.y + enemy.height/2) - (this.y + this.height/2 + 20);
                const distance = Math.sqrt(dx*dx + dy*dy);
                if(distance < enemy.width/3 + this.width/3){
                    gameOver = true;
                }
            })
            // sprite animation
            if(this.frameTimer > this.frameInterval){

                if(this.frameX >= this.maxFrame) this.frameX = 0;
                else this.frameX++;
                this.frameTimer = 0;
            } else{
                this.frameTimer += deltaTime;
            }

            // move controls
            if(input.keys.indexOf('ArrowRight') > -1){
                this.speed = 5;
            } else if(input.keys.indexOf('ArrowLeft') > -1){
                this.speed = -5;
            }
            
            else if((input.keys.indexOf('ArrowUp') > -1 || input.keys.indexOf('swipe up') > -1) && this.onGround()){
                this.vy -= 20;
            }
            else{
                this.speed = 0;
            }
            
            //horizontal movement
            this.x += this.speed;
            if(this.x < 0) this.x = 0;
            else if(this.x > this.gameWidth) this.x = this.gameWidth - this.width;

            //vertical movement
            this.y += this.vy;

            if(!this.onGround()){
                this.vy += this.weight;

                // there are only 5 frame on Vertical Frame for Player!
                this.maxFrame = 5;
                this.frameY = 1;
            }
            else{
                this.vy = 0;
                this.maxFrame = 8;
                this.frameY = 0;
            }

            if(this.y > this.gameHeight - this.height) this.y = this.gameHeight - this.height;
        }
        onGround(){
            return this.y >= this.gameHeight - this.height;
        }
    }

    class Background{
        constructor(gameWidth, gameHeight){
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
            this.image = document.getElementById('backgroundImage');
            this.x = 0;
            this.y = 0;
            this.width = 2400;
            this.height = 720;

            this.speed = 5;
        }   
        restart(){
            this.x = 0;
        }
        draw(context){
            context.drawImage(this.image, this.x, this.y, this.width, this.height);

            // Draw Additional Image for Endless Background
            context.drawImage(this.image, this.x + this.width - this.speed, this.y, this.width, this.height);
        }
        update(){
            this.x -= this.speed;
            
            // If BackgroundImage Scrolled Over!
            if(this.x < 0 - this.width) this.x = 0;
        }
    }

    class Enemy {
        constructor(gameWidth, gameHeight){
            this.gameWidth = gameWidth;
            this.gameHeight = gameHeight;
            this.width = 160;
            this.height = 119;
            this.image = document.getElementById('enemyImage');

            this.x = this.gameWidth;
            this.y = this.gameHeight - this.height;

            this.frameX = 0;
            this.maxFrame = 5;

            // fps : frame per second
            this.fps = 20;
            this.frameTimer = 0;
            this.frameInterval = 1000/this.fps;

            this.speed = 8;
            this.markedForDeletion = false;
        }
        draw(context){
            // Source Y : Hard Coding (there is only 1 line!)
            context.drawImage(this.image, this.frameX * this.width, 0, this.width, this.height, this.x, this.y, this.width, this.height);
        }
        update(deltaTime){
            if(this.frameTimer > this.frameInterval){
                if(this.frameX >= this.maxFrame) this.frameX = 0;
                else this.frameX++;
                this.frameTimer = 0;
            } else{
                this.frameTimer += deltaTime;
            }
            this.x -= this.speed;
            if(this.x < 0 - this.width){
                this.markedForDeletion = true;
                score++;
            }
        }
    }

    function handleEnemies(deltaTime){
        // Handle Mutiple Enemies (Animate, Remove ...)

        if(enemyTimer > enemyInterval + randomEnemyInterval){
            enemies.push(new Enemy(canvas.width, canvas.height));
            enemyTimer = 0;
        }else{
            enemyTimer += deltaTime;
        }
        enemies.forEach(enemy => {
            enemy.draw(ctx);
            enemy.update(deltaTime);
        });
        enemies = enemies.filter(enemy => !enemy.markedForDeletion);
    }

    function displayStatusText(context){
        // Display Score or Game Over Message
        if(!gameOver){
            context.textAlign = 'left';
            context.font = '40px arial';
            context.fillStyle = 'black';
            context.fillText('Score : ' + score, 20, 50);
    
            context.fillStyle = 'white';
            context.fillText('Score : ' + score, 23, 53);
        } else {
            context.font = 'bold 60px arial';
            context.textAlign = 'center';
            context.strokeStyle = 'white';
            context.strokeText('GAME OVER', canvas.width/2, 100);

            context.font = 'italic 25px arial';
            context.fillStyle = 'white';
            context.fillText('Your Score is ' + score + ', Try Again!', canvas.width/2, 130);

            context.font = '30px arial';
            context.fillStyle = 'black';
            context.fillText('Press Enter Key or Swipe Down to Restart', canvas.width/2 - 2, canvas.height - 52);

            context.font = '30px arial';
            context.fillStyle = 'gold';
            context.fillText('Press Enter Key or Swipe Down to Restart', canvas.width/2, canvas.height - 50);
        }

    }

    function restartGame(){
        player.restart();
        background.restart();

        enemies = [];
        score = 0;
        gameOver = false;

        animate(0);
    }

    function toggleFullScreen(){
        if(!document.fullscreenElement) {
            canvas.requestFullscreen().catch(err =>{
                alert(`Error, Cannot enable Full-Screen Mode : ${err.message}`);
            });
        } else{
            document.exitFullscreen();
        }
    }

    fullScreenButton.addEventListener('click', toggleFullScreen);

    const input = new InputHandler();
    const player = new Player(canvas.width, canvas.height);
    const background = new Background(canvas.width, canvas.height);

    let lastTime = 0;
    let enemyTimer = 0;
    let enemyInterval = 1000;
    let randomEnemyInterval = Math.random() * 1000 + 500;

    function animate(timeStamp){
        //Main Animation Loop
        const deltaTime = timeStamp - lastTime;
        lastTime = timeStamp;
        
        ctx.clearRect(0,0,canvas.width, canvas.height);

        // To Show Player : Draw BG First
        background.draw(ctx);
        background.update();

        player.draw(ctx);
        player.update(input, deltaTime , enemies);

        handleEnemies(deltaTime);
        displayStatusText(ctx);
        if(!gameOver) requestAnimationFrame(animate);
    }
    animate(0);
});