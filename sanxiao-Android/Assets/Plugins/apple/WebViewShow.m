
#define WEB_BTN_PADDING_LEFT_PERCENT 0.01
#define WEB_BTN_PADDING_RIGHT_PERCENT 0.01
#define WEB_BTN_PADDING_TOP_PERCENT 0.01
#define WEB_BTN_PADDING_BUTTON_PERCENT 0.01


#define WEB_BTN_WIDTH_PIX 137
#define WEB_BTN_HEIGHT_PIX 59
#define WEB_BTN_PERCENT 0.08


#define WEB_SPLITER_HEIGHT_PERCENT 0.01

#import "WebViewShow.h"

#import "UnityAppController.h"
static WebViewShow *_instance;

@implementation WebViewShow

+(WebViewShow *) sharedInstance{
    if(_instance == nil){
        _instance = [[WebViewShow alloc] init];
        [_instance initView];
        _instance->_superView = [UnityAppController sharedView];
    }
    return _instance;
}

-(void) openWebPage:(NSString *) url{
    [url retain];
    if (_url) {
        [_url release];
    }
    _url = url;
    [_superView addSubview:_view];
    [self show];
}
/**
 
 得到按钮的宽度，value应该是填写屏幕的高度值（pix）
 */
-(float) getBtnWidth :(int) value{
    return value * WEB_BTN_PERCENT * WEB_BTN_WIDTH_PIX * 0.01;
}
/**
 
 得到按钮的高度，value应该是填写屏幕的高度值（pix）
 */

-(float) getBtnHeight:(int) value{
    return value * WEB_BTN_PERCENT * WEB_BTN_HEIGHT_PIX * 0.01;
}

-(float) getHeaderHeight:(int) value{
    return value * (WEB_BTN_PADDING_TOP_PERCENT + WEB_BTN_PADDING_BUTTON_PERCENT)
    + [self getBtnHeight:value];
}

-(void) initView {
    
    CGRect currentScreen = [[UIScreen mainScreen] bounds];
    int width = currentScreen.size.width;
    int height = currentScreen.size.height;
    
    NSLog(@"screen:width:%d,height:%d",width,height);
    
    _view = [[UIView alloc] initWithFrame:CGRectMake(0, 0, width, height)];
    
    
    UIView *header = [[UIView alloc] initWithFrame:CGRectMake(0, 0, width,[self getHeaderHeight:height])];
    
    [header setBackgroundColor:[UIColor colorWithRed:0.56f green:0.32f blue:0.15f alpha:1.0]];
    
    [_view addSubview:header];
    
    UIView *spliter = [[UIView alloc] initWithFrame:CGRectMake(0, [self getHeaderHeight:height], width, height * WEB_SPLITER_HEIGHT_PERCENT)];
    
    [spliter setBackgroundColor:[UIColor colorWithRed:1.0f green:0.69f blue:0.22f alpha:1.0f]];
    [_view addSubview:spliter];
    

    
    UIButton *backBtn = [[UIButton alloc] init];
    
    [backBtn setBackgroundImage:[UIImage imageNamed:@"web_back_normal.png"] forState:UIControlStateNormal];
    
    backBtn.frame = CGRectMake(width * WEB_BTN_PADDING_LEFT_PERCENT,
                               height * WEB_BTN_PADDING_TOP_PERCENT,
                               [self getBtnWidth:height],
                               [self getBtnHeight:height]
                               );
    
    [backBtn addTarget:self action:@selector(back) forControlEvents:UIControlEventTouchUpInside];
    
    [_view addSubview:backBtn];
    
    [backBtn release];
    
    UIButton *refreshBtn = [[UIButton alloc] init];
    
    [refreshBtn setBackgroundImage:[UIImage imageNamed:@"web_update_normal.png"] forState:(UIControlStateNormal)];
    
    refreshBtn.frame = CGRectMake(width - width * WEB_BTN_PADDING_RIGHT_PERCENT - [self getBtnWidth:height],
                                  height * WEB_BTN_PADDING_TOP_PERCENT,
                                  [self getBtnWidth:height],
                                  [self getBtnHeight:height]);
    
    [refreshBtn addTarget:self action:@selector(update) forControlEvents:UIControlEventTouchUpInside];
    
    [_view addSubview:refreshBtn];
    [refreshBtn release];
    
    _webview = [[UIWebView alloc] initWithFrame:CGRectMake(0, [self getHeaderHeight:height] + height * WEB_SPLITER_HEIGHT_PERCENT, width,height -[self getHeaderHeight:height] - height * WEB_SPLITER_HEIGHT_PERCENT)];
    
    _webview.delegate = self;
    
    [_view addSubview:_webview];
    
    _indicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    [_indicator setFrame:CGRectMake(0, 0, 256, 256)];
    [_indicator setColor:[UIColor blackColor]];
    [_indicator setTintColor:[UIColor blackColor]];
    _indicator.center = _view.center;
    [_view addSubview:_indicator];
    
    
}

-(void) show{
    
    NSLog(@"show %@", _url);
    
    if([_indicator isAnimating]){
        
        NSLog(@"不要刷新过快哦。");
    }else{
        
        NSURL *nsurl = [NSURL URLWithString:_url];
        NSURLRequest *request = [NSURLRequest requestWithURL:nsurl];
        [_webview loadRequest:request];
    }
    
}
-(void) registerListener:(id<WebPageLoaded>) listener{
    _listener = listener;
}
-(void) removeListener{
    _listener = nil;
}

//back to unity
-(void) back{
    [_view removeFromSuperview];
}

-(void) update{
    [self show];
}

- (void) webViewDidStartLoad:(UIWebView *)webView{
    [_indicator startAnimating];
    NSString *url = webView.request.URL.absoluteString;
    NSLog(@"start load......%@",url);
    
}

- (void) webViewDidFinishLoad:(UIWebView *)webView{
    [_indicator stopAnimating];
    NSString *url = webView.request.URL.absoluteString;
    NSLog(@"finish load......%@",url);
    if(_listener != nil){
        [_listener PageLoaded:url];
    }
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error{
    [_indicator stopAnimating];
    [self cannotConnectToNet];
}

-(void) cannotConnectToNet{
    NSLog(@"can not connect to net");
    
}

@end
