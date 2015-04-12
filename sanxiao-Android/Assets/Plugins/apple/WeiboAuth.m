//
//  WeiboAuth.m
//  Unity-iPhone
//
//  Created by 赵 之韵 on 14-1-7.
//
//

#import "WeiboAuth.h"
#import "WebViewShow.h"
#import "NSString+URLEncoding.h"
#import "NSString+CStringCopy.h"

void _weiboAuth(const char* appKey,const char* redirectUrl,int authType,int responseType){
    
    NSString *appKeyStr = [NSString stringWithUTF8String:appKey];
    
    NSString *redirectUrlStr = [NSString stringWithUTF8String:redirectUrl];
    
    WeiboAuth *weibo = [[WeiboAuth alloc] initWithAuthType:(enum AuthType)authType responseType:Code appKey:appKeyStr redirectUrl:redirectUrlStr];
    [weibo startAuth];
    
}

@implementation WeiboAuth

-(id)initWithAuthType:(enum AuthType)authType responseType:(enum ResponseType)aResponseType appKey:(NSString *)aAppKey redirectUrl:(NSString *)aRedirectUrl{
    self = [self init];
    _authType = authType;
    _responseType = aResponseType;
    _appKey = aAppKey;
    [aRedirectUrl retain]; 
    _redirectUrl = aRedirectUrl;
    return self;
}
-(void) startAuth{
    
    NSString *urlPrefix;
    
    switch (_authType) {
        case Sina:
            urlPrefix = @"https://open.weibo.cn/oauth2/authorize";
            break;
        case Tecent:
            urlPrefix = @"https://open.t.qq.com/cgi-bin/oauth2/authorize";
            break;
        case Renren:
            urlPrefix = @"https://graph.renren.com/oauth/authorize";
            break;
        default:
            break;
    }
    
    NSString *response;
    switch (_responseType) {
       
        case Code:
            response = @"code";
            break;
        case Token:
            response = @"token";
            break;
            
        default:
            break;
    }
    
    
    NSString *url =[NSString stringWithFormat:@"%@?client_id=%@&redirect_uri=%@&response_type=%@&display=mobile",urlPrefix,_appKey,[_redirectUrl urlEncodeUsingEncoding:NSUTF8StringEncoding],response];
    
    [[WebViewShow sharedInstance] registerListener:self];
    [[WebViewShow sharedInstance] openWebPage:url];
    
}
-(void) PageLoaded:(NSString *)url{
    if([url hasPrefix:_redirectUrl]){
        NSLog(@"auth ok");
    
        NSRange rangeCode = [url rangeOfString:@"code"];
        
        if(rangeCode.length > 0){
            NSString *params = [url substringFromIndex:rangeCode.location];
            NSLog(@"%@",params);
            
            NSArray *array = [params componentsSeparatedByString:@"="];
            
            NSString *str = [NSString stringWithFormat:@"{\"authType\":%d,\"code\":\"%@\"}",(int)_authType,array[1]];
            
            NSLog(@"%@",str);
            
            UnitySendMessage("IOSReceiver", "WeiboAuthResult", [str cStringCopy]);
            
        }
        [[WebViewShow sharedInstance] back];
    }
    
}


@end
