<template>
	<view class="container">
		<!-- #ifndef MP-WEIXIN -->
		<view class="tui-status-bar"></view>
		<view class="tui-header">
			<view>MiCake 实验仓库</view>
			<tui-icon name="shut" :size="26" @click="back"></tui-icon>
		</view>
		<!-- #endif -->
		<view class="tui-page-title">登录</view>
		<view class="tui-form">
			<view class="tui-view-input">
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="mobile" color="#6d7a87" :size="20"></tui-icon>
						<input :adjust-position="false" :value="mobile" placeholder="请输入手机号" placeholder-class="tui-phcolor" type="number" maxlength="11" @input="inputMobile" />
						<view class="tui-icon-close" v-show="mobile" @tap="clearInput(1)">
							<tui-icon name="close-fill" :size="16" color="#bfbfbf"></tui-icon>
						</view>
					</view>
				</tui-list-cell>
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="shield" color="#6d7a87" :size="20"></tui-icon>
						<input placeholder="请输入验证码" placeholder-class="tui-phcolor" :value="code" type="text" maxlength="6" @input="inputCode" />
						<view class="tui-btn-send" @tap="sendCode" :class="{ 'tui-gray': isSend }" :hover-class="isSend ? '' : 'tui-opcity'" :hover-stay-time="150">{{ btnSendText }}</view>
					</view>
				</tui-list-cell>
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="pwd" color="#6d7a87" :size="20"></tui-icon>
						<input :adjust-position="false" :value="password" placeholder="请输入密码" :password="true" placeholder-class="tui-phcolor" type="text" maxlength="36" @input="inputPwd" />
						<view class="tui-icon-close" v-show="password" @tap="clearInput(2)">
							<tui-icon name="close-fill" :size="16" color="#bfbfbf"></tui-icon>
						</view>
					</view>
				</tui-list-cell>
			</view>
			<view class="tui-cell-text">
				<view class="tui-color-primary" hover-class="tui-opcity" :hover-stay-time="150" @tap="forgetPwd">忘记密码？</view>
				<view hover-class="tui-opcity" :hover-stay-time="150">
					没有账号？
					<navigator url="/pages/login/reg" hover-class="opcity" :hover-stay-time="150">
						<text class="tui-color-primary">注册</text>
					</navigator>
				</view>
			</view>
			<view class="tui-btn-box">
				<tui-button :disabledGray="true" :disabled="disabled" :shadow="true" shape="circle" @tap="login">登录</tui-button>
			</view>
		</view>
		<view class="tui-login-way" v-if="!popupShow">
			<view hover-class="tui-opcity" :hover-stay-time="150" @tap="showOtherLogin">其他方式登录</view>
		</view>
		<tui-bottom-popup :mask="false" backgroundColor="transparent" :show="popupShow">
			<view class="tui-auth-login">
				<!-- #ifdef APP-PLUS || MP-WEIXIN || H5 -->
				<view class="tui-icon-platform" hover-class="tui-opcity" :hover-stay-time="150" @tap="loginWithWechat">
					<image src="/static/images/share/icon_wechat.png" class="tui-login-logo" />
				</view>
				<!-- #endif -->
			</view>
		</tui-bottom-popup>

		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import { Action, Mutation, State } from "vuex-class";
import { UserStoreKey } from "@/store/store-keys";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiBottomPopup from "@/components/thorui/tui-bottom-popup/tui-bottom-popup.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import uniHelper, { thorUiHelper } from '../../common/uniHelper';
import { LoginDto, LoginResultDto, WeChatLoginDto } from "@/models/apiModel";
import { MiCakeApiModel } from "@/common/environment";

const namespace = UserStoreKey.nameSpace;

@Component({
	components: {
		tuiListCell,
		tuiIcon,
		tuiButton,
		tuiBottomPopup,
		tuiTips
	},
})
export default class extends Vue {

	private _disabled: boolean = false;
	public get disabled(): boolean {
		return this._disabled;
	}

	public mobile: string = "";
	public password: string = "";
	public code: string = '';
	public popupShow: boolean = false;
	public isSend: boolean = false;
	btnSendText: string = '获取验证码';

	@Action(UserStoreKey.actions_loginSuccess, { namespace }) public loginSuccess!: Function;
	@Mutation(UserStoreKey.mutations_saveUserInfo, { namespace }) public saveUserInfo!: Function;

	public clearInput(type: number) {
		if (type == 1) {
			this.mobile = '';
		} else {
			this.password = '';
		}
	}

	public forgetPwd() {
		thorUiHelper.showTips(this.$refs.toast, '不准忘记密码。就算忘记了，数据库又不在我这儿╰（‵□′）╯');
	}

	public inputMobile(e: any) {
		this.mobile = e.detail.value;
	}

	public inputPwd(e: any) {
		this.password = e.detail.value;
	}

	public inputCode(e: any) {
		this.code = e.detail.value;
	}

	public sendCode() {
		this.isSend = true;

		thorUiHelper.showTips(this.$refs.toast, '直接输入 micake 就行啦~', 2000, 'green');

		setTimeout(() => {
			this.isSend = false;
		}, 5000);
	}

	public async login() {
		if (!uniHelper.validator.isMobile(this.mobile)) {
			thorUiHelper.showTips(this.$refs.toast, '貌似手机号码不正确呀~');
			return;
		}

		if (uniHelper.validator.isNullOrEmpty(this.password)) {
			thorUiHelper.showTips(this.$refs.toast, '貌似还没有输入密码哦~');
			return;
		}

		var loginInfo = new LoginDto();
		loginInfo.phone = this.mobile;
		loginInfo.password = this.password;
		loginInfo.code = this.code;

		try {
			uniHelper.showLoading();
			let result = await this.$httpClient.post<MiCakeApiModel<LoginResultDto>>('/User/Login', loginInfo);
			await uniHelper.hideLoading(2000);

			if (result.isError) {
				thorUiHelper.showTips(this.$refs.toast, result.message ?? "存在错误哦~");
				return;
			}

			var loginResult = result.result!;
			if (!loginResult.hasUser) {
				thorUiHelper.showTips(this.$refs.toast, '您还没有注册哟~赶快注册一个账号吧');
				return;
			}

			this.loginSuccess(result.result!.accessToken);
			let userInfo = result.result!.userInfo!;
			this.saveUserInfo({ id: userInfo.id, name: userInfo.name, avatar: userInfo.avatar, mobile: this.mobile });

			uni.navigateBack(); //回跳到上一步页面
		} catch (error) {
			console.log(error);
		}
	}

	public async loginWithWechat() {
		//#ifndef MP-WEIXIN
		thorUiHelper.showTips(this.$refs.toast, '这个按钮只能在微信小程序中才能发挥洪荒之力呀~', 2000, 'green');
		return;
		//#endif

		wx.login({
			success: this.WechatLoginSuccessCallBack
		});
	}

	private async WechatLoginSuccessCallBack(res: { code: string, errMsg: string }) {
		//向aspnet core后台进行身份验证
		uniHelper.showLoading();
		let result = await this.$httpClient.get<MiCakeApiModel<WeChatLoginDto>>('/signin-wechat', { 'code': res.code });
		await uniHelper.hideLoading(2000);

		if (result.isError) {
			thorUiHelper.showTips(this.$refs.toast, result.message ?? "存在错误哦~");
			return;
		}

		if (!result.result!.hasUser) {
			uni.navigateTo({ url: `/pages/login/regWithWechat?key=${result.result!.openSessionKey}` });
			return;
		}

		this.loginSuccess(result.result!.accessToken);
		let userInfo = result.result!.userInfo!;
		this.saveUserInfo({ id: userInfo.id, name: userInfo.name, avatar: userInfo.avatar, mobile: this.mobile });
		uni.navigateBack(); //回跳到上一步页面
	}

	public showOtherLogin() {
		//打开后 不再关闭
		this.popupShow = true;
	}

	public back() {
		uni.navigateBack({
			animationType: 'pop-out',
			animationDuration: 300
		});
	}
}
</script>

<style lang="scss">
.container {
	.tui-status-bar {
		width: 100%;
		height: var(--status-bar-height);
	}

	.tui-header {
		width: 100%;
		padding: 40rpx;
		display: flex;
		align-items: center;
		justify-content: space-between;
		box-sizing: border-box;
	}

	.tui-page-title {
		width: 100%;
		font-size: 48rpx;
		font-weight: bold;
		color: $uni-text-color;
		line-height: 42rpx;
		padding: 40rpx;
		box-sizing: border-box;
	}

	.tui-form {
		padding-top: 50rpx;

		.tui-view-input {
			width: 100%;
			box-sizing: border-box;
			padding: 0 40rpx;

			.tui-cell-input {
				width: 100%;
				display: flex;
				align-items: center;
				padding-top: 48rpx;
				padding-bottom: $uni-spacing-col-base;

				input {
					flex: 1;
					padding-left: $uni-spacing-row-base;
				}

				.tui-icon-close {
					margin-left: auto;
				}

				.tui-gray {
					color: $uni-text-color-placeholder;
				}
			}
		}

		.tui-cell-text {
			width: 100%;
			padding: $uni-spacing-col-lg $uni-spacing-row-lg;
			box-sizing: border-box;
			font-size: $uni-font-size-sm;
			color: $uni-text-color-grey;
			display: flex;
			align-items: center;
			justify-content: space-between;

			.tui-color-primary {
				color: $uni-color-primary;
			}
		}

		.tui-btn-box {
			width: 100%;
			padding: 0 $uni-spacing-row-lg;
			box-sizing: border-box;
			margin-top: 80rpx;
		}
	}

	.tui-login-way {
		width: 100%;
		font-size: 26rpx;
		color: $uni-color-primary;
		display: flex;
		justify-content: center;
		position: fixed;
		left: 0;
		bottom: 80rpx;

		view {
			padding: 12rpx 0;
		}
	}

	.tui-auth-login {
		width: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		padding-bottom: 80rpx;
		padding-top: 20rpx;

		.tui-icon-platform {
			width: 90rpx;
			height: 90rpx;
			display: flex;
			align-items: center;
			justify-content: center;
			position: relative;

			&::after {
				content: "";
				position: absolute;
				width: 200%;
				height: 200%;
				transform-origin: 0 0;
				transform: scale(0.5, 0.5) translateZ(0);
				box-sizing: border-box;
				left: 0;
				top: 0;
				border-radius: 180rpx;
				border: 1rpx solid $uni-text-color-placeholder;
			}
		}

		.tui-login-logo {
			width: 60rpx;
			height: 60rpx;
		}
	}
}
</style>
