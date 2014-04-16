### MultiWaveView

C#实现的多波形实时显示解决方案（基于VS2005环境）

=

#### 使用介绍

首先需要引入MultiWaveView.dll（如果直接通过源代码集成则不用）。 两个类，一个为MultiWaveView，是主要的波形控件，用于显示波形。另一个为WaveProperty，是波形控件中的波属性。 一个MultiWaveView控件中可以有多个WaveProperty（设置waves属性）。

如果使用设计器的话可以直接将MultiWaveView控件拖拽到窗体设计界面上，主要设置一下waves属性，在waves属性设置对话框中设置你需要显示的波形条数，以及每条波的各种属性（如颜色、最大值、最小值等等）。

之后可以通过定时器对MultiWaveView控件的Values进行赋值即可实时显示波形（注意Values为一维double数组，数组的长度和waves中波形的条数相同），设计器的例子可以参考MultiWaveViewDemo工程：

```CSharp
double v1 = Math.Sin(a);
double v2 = Math.Cos(a);
double v3 = Math.Tan(a);
mwvWaves.Values = new double[]{v1,v2,v3};
```

如果不用设计器，而是用代码来进行布局的话可以参考下面的例子：


```CSharp
// 实例波形控件化
this.mwv = new Sin.UI.MultiWaveView();
this.mwv.Dock = DockStyle.Fill;


// 初始化两条波形
Sin.UI.WaveProperty wp1 = new Sin.UI.WaveProperty();
wp1.Color = Color.Red;
wp1.Name = "第一条波形";

Sin.UI.WaveProperty wp2 = new Sin.UI.WaveProperty();
wp2.Color = Color.Blue;
wp2.Name = "第二条波形";

// 添加波形到控件
mwv.Waves = new Sin.UI.WaveProperty[] { wp1, wp2 };

// 添加控件到窗体
this.Controls.Add(mwv);
```

后面实时更新波形数据和上面介绍的一样。

=

#### 部分使用截图
* MultiWaveViewDemo运行截图
> ![image](https://raw.githubusercontent.com/sintrb/MultiWaveView/master/MultiWaveViewDoc/screenshots/waves.png)

* 上面提到的代码方式添加控件运行截图
> ![image](https://raw.githubusercontent.com/sintrb/MultiWaveView/master/MultiWaveViewDoc/screenshots/codestyle.png)


=

#### 版本更新说明

##### v1.0 (2014-04-15)

* 发布到GitHub上的第一个版本，已经有基本的功能，并在实际项目中得到使用
