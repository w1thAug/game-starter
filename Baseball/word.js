// 랜덤



var answer = [0, 0, 0, 0, 0];
var tryCount = 0;

var isEmpty = function(value){
    if( value == "" || value == null || value == undefined || 
        ( value != null && typeof value) == "object" && !Object.keys(value).length ){
      return true
    }else{
      return false
    }
  };


for(let n = 0; n < 5; n++){
    var rand = Math.floor(Math.random() * 10);
    if(rand == 0) answer[n] = rand + 1;
    else answer[n] = rand;
}

console.log("Answer : " + answer);

document.querySelector('button').addEventListener('click', function(){
    var input = document.querySelectorAll('.input');
    var isAllInt = true;
    var correctNum = 0;
    var checkAnswer = [0, 0, 0, 0, 0];
    
    for(let k = 0; k < answer.length; k++)
        checkAnswer[k] = answer[k];
    
    for(let i = (5 * tryCount); i < (5 * tryCount) + 5; i++){
        if(isNaN(input[i].value)){
            isAllInt = false;
        }
        
        else if(isEmpty(input[i].value)){
            isAllInt = false;
        }
        
    }

    if(!isAllInt){
        //alert("모든 칸을 숫자로만 채워주세요!" + test);
    }
    else{
        
        for(let i = (5 * tryCount); i <= (5 * tryCount) + 4; i++){
            
            var inputValue = input[i].value;
            
            if(inputValue == checkAnswer[i % 5]){
                input[i].style.background = 'lightgreen';
                checkAnswer[i % 5] = 0;
                
                correctNum++;
            }
        }
        
        for(let i = (5 * tryCount); i <= (5 * tryCount) + 4; i++){
            
            var inputValue = input[i].value;
            var isContain = false;

            if(checkAnswer[i % 5] != 0){
                checkAnswer.forEach(e => {
                    if(e == inputValue) isContain = true;
                });

                if(isContain){
                    input[i].style.background = 'lightyellow';
                }
                else {
                    input[i].style.background = 'pink';
                }
            }
        }

        var template = `<div>
        <input class = "input">
        <input class = "input">
        <input class = "input">
        <input class = "input">
        <input class = "input">
        </div>`;
        
        if(correctNum < 5){
            tryCount++;
            document.querySelector('body').insertAdjacentHTML('beforeend', template);
        }
        else{
            alert(tryCount + "번 만에 정답!");
        }
        
        
    }

})
