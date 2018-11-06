// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9031,x:33294,y:32777,varname:node_9031,prsc:2|emission-7910-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:9006,x:32048,y:32744,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:node_9006,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5726,x:32263,y:32707,varname:node_5726,prsc:2,ntxv:0,isnm:False|TEX-9006-TEX;n:type:ShaderForge.SFN_VertexColor,id:4541,x:32263,y:32852,varname:node_4541,prsc:2;n:type:ShaderForge.SFN_Fresnel,id:4485,x:32218,y:33023,varname:node_4485,prsc:2|EXP-3045-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3045,x:32014,y:33093,ptovrint:False,ptlb:node_3045,ptin:_node_3045,varname:node_3045,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:4283,x:32445,y:33009,varname:node_4283,prsc:2|VAL-4485-OUT,EXP-2104-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2104,x:32218,y:33217,ptovrint:False,ptlb:Fresnel Power,ptin:_FresnelPower,varname:node_2104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Add,id:7910,x:33046,y:32856,varname:node_7910,prsc:2|A-919-RGB,B-8358-OUT;n:type:ShaderForge.SFN_Multiply,id:3221,x:32609,y:32946,varname:node_3221,prsc:2|A-4541-RGB,B-4283-OUT;n:type:ShaderForge.SFN_Color,id:919,x:32480,y:32663,ptovrint:False,ptlb:node_919,ptin:_node_919,varname:node_919,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:8358,x:32839,y:32977,varname:node_8358,prsc:2|A-3221-OUT,B-6728-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6728,x:32678,y:33143,ptovrint:False,ptlb:Fresnel Intensity,ptin:_FresnelIntensity,varname:node_6728,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;proporder:9006-3045-2104-919-6728;pass:END;sub:END;*/

Shader "Custom/Fresnel" {
    Properties {
        _MainTexture ("Main Texture", 2D) = "white" {}
        _node_3045 ("node_3045", Float ) = 1
        _FresnelPower ("Fresnel Power", Float ) = 5
        _node_919 ("node_919", Color) = (0,0,0,1)
        _FresnelIntensity ("Fresnel Intensity", Float ) = 1.5
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _node_3045;
            uniform float _FresnelPower;
            uniform float4 _node_919;
            uniform float _FresnelIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = (_node_919.rgb+((i.vertexColor.rgb*pow(pow(1.0-max(0,dot(normalDirection, viewDirection)),_node_3045),_FresnelPower))*_FresnelIntensity));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
