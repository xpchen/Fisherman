import { _decorator, Component, director } from 'cc';

const { ccclass } = _decorator;

@ccclass('HomeUIController')
export class HomeUIController extends Component {
    onClickStartGame() {
        console.log('点击了开始游戏，准备进入 FishingScene');
        director.loadScene('FishingScene');
    }
}