<template>
	<view class="container">
		<view class="top-container">
			<image class="bg-img" src="/static/images/my/mine_bg_3x.png" />
			<view @tap="logout" class="logout" hover-class="opcity" :hover-stay-time="150">
				<image class="logout-img" src="/static/images/my/icon_out_3x.png" v-show="isLogin" />
				<text class="logout-txt" v-show="isLogin" @tap="loginOut">退出</text>
			</view>
			<view v-show="!isLogin" class="user-wrapper">
				<navigator url="/pages/login/login" hover-class="opcity" :hover-stay-time="150" class="user">
					<image class="avatar-img" src="/static/images/my/mine_def_touxiang_3x.png" />
					<text class="user-info-mobile">请登录</text>
				</navigator>
			</view>
			<view v-show="isLogin" class="user">
				<image class="avatar-img" src="/static/images/my/mine_def_touxiang_3x.png" />
				<view class="user-info-mobile">
					<text>{{ userName }}</text>
					<!-- <view class="edit-img" hover-class="opcity" :hover-stay-time="150" @tap="edit">
						<image src="/static/images/my/mine_icon_bianji_3x.png" />
					</view>-->
				</view>
			</view>
		</view>

		<view class="middle-container">
			<view @tap="tapMiCake" data-index="1" class="middle-item" hover-class="opcity" :hover-stay-time="150">
				<image class="ticket-img" src="/static/images/my/go.png" />
				<text class="middle-tag">MiCake</text>
			</view>
			<!-- #ifdef APP-PLUS || MP -->
			<view @tap="github(1)" class="middle-item" hover-class="opcity" :hover-stay-time="150">
				<image class="car-img" src="/static/images/my/github_3x.png" />
				<text class="middle-tag">GitHub</text>
			</view>
			<!-- #endif -->

			<!-- #ifdef H5 -->
			<view @tap="github(2)" class="middle-item" hover-class="opcity" :hover-stay-time="150">
				<image class="car-img" src="/static/images/my/github_3x.png" />
				<text class="middle-tag">GitHub</text>
			</view>
			<!-- #endif -->
		</view>

		<view class="bottom-container">
			<view class="ul-item">
				<view @tap="appreciate" class="item" hover-class="opcity" :hover-stay-time="150">
					<image class="item-img" src="/static/images/my/reward.png" />
					<text class="item-name">赞赏</text>
				</view>
				<view class="item" hover-class="opcity" :hover-stay-time="150" @tap="feedback">
					<button open-type="feedback" class="btn-feedback"></button>
					<image class="item-img" src="/static/images/my/feedback.png" />
					<text class="item-name">反馈</text>
				</view>
				<view @tap="logInfo" data-index="3" class="item" hover-class="opcity" :hover-stay-time="150">
					<image class="item-img" src="/static/images/my/log.png" />
					<text class="item-name">日志</text>
				</view>
			</view>
		</view>

		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
	</view>
</template> 

<script lang="ts">

import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import { State, Action } from "vuex-class";
import uniHelper, { thorUiHelper } from '../../common/uniHelper';
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import { UserStoreKey } from '../../store/store-keys';

const namespace = UserStoreKey.nameSpace;

@Component({
	components: {
		tuiTips
	}
})
export default class extends Vue {

	@State(UserStoreKey.state_isLogin, { namespace })
	public isLogin!: boolean;

	@Action(UserStoreKey.actions_loginOut, { namespace })
	public loginOutAction!: Function;

	public userName: string = 'Hello MiCaker';

	public edit() { }

	public tapMiCake(){
		thorUiHelper.showTips(this.$refs.toast, '希望在Github得到您的star',2000,'green');
	}

	public appreciate() {
		thorUiHelper.showTips(this.$refs.toast, 'comming soon.');
	}

	public feedback() {
		thorUiHelper.showTips(this.$refs.toast, 'comming soon.');
	}

	public logInfo() {
		thorUiHelper.showTips(this.$refs.toast, 'comming soon.');
	}

	public github(type: number) {
		if (type == 2) {
			uniHelper.showToast('即将跳转到Github....');

			setTimeout(() => {
				location.href = 'https://github.com/uoyoCsharp/MiCake';
			}, 1000);
		}

		uni.setClipboardData({
			data: 'https://github.com/uoyoCsharp/MiCake',
			success: () => {
				uniHelper.showToast('链接已经复制,可以在浏览器里去打开哟~');
			}
		});
	}

	public loginOut() {
		thorUiHelper.showTips(this.$refs.toast, '啊~！再见朋友~', 2000, 'green');
		this.loginOutAction();
	}
}
</script>

<style>
.container {
	position: relative;
	padding-bottom: 100rpx;
}

.top-container {
	height: 440rpx;
	position: relative;
	display: flex;
	flex-direction: column;
}

.bg-img {
	position: absolute;
	width: 100%;
	height: 440rpx;
}

.logout {
	width: 110rpx;
	height: 36rpx;
	display: flex;
	flex-direction: row;
	align-items: center;
	margin: 42rpx 0 24rpx 32rpx;
	position: relative;
	z-index: 2;
}

.logout-img {
	width: 36rpx;
	height: 36rpx;
	margin-right: 11rpx;
}

.logout-txt {
	font-size: 28rpx;
	color: #fefefe;
	line-height: 28rpx;
}

.user-wrapper {
	display: flex;
	justify-content: center;
	position: relative;
	z-index: 2;
}

.user {
	display: flex;
	flex-direction: column;
	justify-content: center;
	position: relative;
	z-index: 2;
}

.avatar-img {
	width: 160rpx;
	height: 160rpx;
	border-radius: 50%;
	align-self: center;
}

.user-info {
	display: flex;
	flex-direction: row;
	margin-top: 30rpx;
	align-items: center;
}

.user-info-mobile {
	margin-top: 30rpx;
	position: relative;
	font-size: 28rpx;
	color: #fefefe;
	line-height: 28rpx;
	align-self: center;
	padding: 0 50rpx;
}

.edit-img {
	position: absolute;
	width: 42rpx;
	height: 42rpx;
	right: 0;
	bottom: -4rpx;
}

.edit-img > image {
	width: 42rpx;
	height: 42rpx;
	padding-left: 25rpx;
}

.middle-container {
	height: 138rpx;
	display: flex;
	flex-direction: row;
	align-items: center;
	border-radius: 10rpx;
	background-color: #ffffff;
	margin: -30rpx 30rpx 26rpx 30rpx;
	box-shadow: 0 15rpx 10rpx -15rpx #efefef;
	position: relative;
	z-index: 2;
}

.middle-item {
	height: 100%;
	display: flex;
	flex-direction: row;
	align-items: center;
}

.ticket-img {
	width: 80rpx;
	height: 80rpx;
	margin-left: 65rpx;
}

.middle-tag {
	font-size: 28rpx;
	color: #333333;
	line-height: 28rpx;
	font-weight: bold;
	padding-left: 22rpx;
}

.car-img {
	width: 80rpx;
	height: 80rpx;
	margin-left: 97rpx;
}

.bottom-container {
	height: 334rpx;
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	padding: 40rpx 74rpx 40rpx 95rpx;
	margin: 0 30rpx;
	background-color: #ffffff;
	border-radius: 10rpx;
	box-sizing: border-box;
	box-shadow: 0 0 10rpx #efefef;
}

.ul-item {
	display: flex;
	flex-direction: row;
	justify-content: space-between;
}

.item {
	display: flex;
	flex-direction: column;
	align-items: center;
	position: relative;
}

.item-img {
	width: 64rpx;
	height: 64rpx;
}

.item-name {
	padding-top: 13rpx;
	font-size: 24rpx;
	color: #666666;
	min-width: 80rpx;
	text-align: center;
}

.btn-feedback {
	background: transparent !important;
	position: absolute;
	height: 100%;
	width: 100%;
	left: 0;
	top: 0;
	border: 0;
}

.btn-feedback::after {
	border: 0;
}
</style>
