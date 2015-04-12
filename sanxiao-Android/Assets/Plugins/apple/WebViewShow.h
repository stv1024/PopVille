//
//  WebViewShow.h
//  Unity-iPhone
//
//  Created by alking sun  on 13-10-9.
//  aijingsun6@gmail.com
//
#import <Foundation/Foundation.h>

@protocol WebPageLoaded <NSObject>
-(void) PageLoaded:(NSString *) url;
@end

@interface WebViewShow: NSObject<UIWebViewDelegate>{
    UIView *_superView;
    UIView *_view;
    UIWebView *_webview;
    UIActivityIndicatorView *_indicator;
    NSString *_url;
    id<WebPageLoaded> _listener;
}

+(WebViewShow *) sharedInstance;

-(void) openWebPage:(NSString *) url;

-(void) back;

-(void) registerListener:(id<WebPageLoaded>) listener;

-(void) removeListener;
@end

